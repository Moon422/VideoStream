using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Extensions.Logging;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Infrastructure.Storage;

namespace VideoStream.Infrastructure.Services;

public class VideoProcessingService : IVideoProcessingService
{
    private readonly IVideoRepository _videoRepository;
    private readonly ISubtitleRepository _subtitleRepository;
    private readonly ILogger<VideoProcessingService> _logger;
    private readonly LocalFileStorageService _storage;

    private readonly int[] _targetHeights = [360, 480, 720, 1080];

    public VideoProcessingService(IVideoRepository videoRepository,
        ISubtitleRepository subtitleRepository,
        ILogger<VideoProcessingService> logger,
        LocalFileStorageService storage)
    {
        _videoRepository = videoRepository;
        _subtitleRepository = subtitleRepository;
        _logger = logger;
        _storage = storage;
    }

    private async Task<string?> ResizeAsync(string filepath, int width, int height, int targetHeight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filepath);

        if (targetHeight > height) return null;

        _logger.LogInformation("Resizing video file: {0} to {1}p", Path.GetFileName(filepath), targetHeight);

        int targetWidth = (int)((float)width / targetHeight * height);
        if (targetWidth % 2 != 0)
            targetWidth++;

        var directory = Path.GetDirectoryName(filepath);
        var fileExt = Path.GetExtension(filepath);
        var targetPath = Path.Join(directory, $"{targetWidth}_{targetHeight}.{fileExt}");

        await FFMpegArguments.FromFileInput(filepath)
            .OutputToFile(targetPath, false, options => options
                .WithVideoCodec(VideoCodec.LibX264)
                .WithSpeedPreset(Speed.Medium)
                .WithConstantRateFactor(23)
                .WithAudioCodec(AudioCodec.Aac)
                .WithAudioBitrate(AudioQuality.Good)
                .WithVideoFilters(filterOptions => filterOptions.Scale(targetWidth, targetHeight))
                .WithFastStart())
            .ProcessAsynchronously();

        _logger.LogInformation("Video file: {0} resized to {1}p", Path.GetFileName(filepath), targetHeight);

        return targetPath;
    }

    private async Task SegmentVideoToHlsAsync(string? filepath, int videoId)
    {
        if (string.IsNullOrWhiteSpace(filepath)) return;

        _logger.LogInformation("Segmenting video file: {0}", Path.GetFileName(filepath));

        var filename = Path.GetFileNameWithoutExtension(filepath);
        var rootDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(filepath)));
        var outputDirectory = Path.Join(rootDirectory, "hls", videoId.ToString(), filename);
        Directory.CreateDirectory(outputDirectory);

        var masterPlaylistPath = Path.Join(outputDirectory, "master.m3u8");

        await FFMpegArguments.FromFileInput(filepath)
            .OutputToFile(masterPlaylistPath, overwrite: true, options => options
                .ForceFormat("hls")
                .WithCustomArgument("-hls_time 10")
                .WithCustomArgument("-hls_list_size 0")
                .WithCopyCodec()
                .WithCustomArgument("filename.m3u8"))
            .ProcessAsynchronously();

        _logger.LogInformation("Segmented video file: {0}", Path.GetFileName(filepath));
    }

    private async Task ProcessVideoAsync(string filepath, int videoId)
    {
        _logger.LogInformation("Processing video file: {0}", Path.GetFileName(filepath));

        var mediaInfo = await FFProbe.AnalyseAsync(filepath);
        var width = mediaInfo.PrimaryVideoStream?.Width ?? 0;
        var height = mediaInfo.PrimaryVideoStream?.Height ?? 0;

        if (width < 0 || height < 0)
        {
            return;
        }

        foreach (var targetHeight in _targetHeights)
        {
            var resizedFilePath = await ResizeAsync(filepath, width, height, targetHeight);
            await SegmentVideoToHlsAsync(resizedFilePath, videoId);
        }

        _logger.LogInformation("Processing video file compplete: {0}", Path.GetFileName(filepath));
    }

    public async Task EnqueueProcessingAsync(int videoId, Stream videoStream)
    {
        var video = await _videoRepository.GetByIdAsync(videoId)
            ?? throw new InvalidOperationException($"Video {videoId} not found");

        // Save source video
        var srcName = $"{Guid.NewGuid():N}.mp4";
        var path = await _storage.SaveVideoAsync(videoId, videoStream, srcName);
        video.FileName = srcName;
        video.FilePath = path;
        video.Status = VideoStatus.Processing;
        await _videoRepository.UpdateAsync(video);

        // In a real system, enqueue a background job to transcode and HLS package
        _logger.LogInformation("Queued processing for video {VideoId}", videoId);

        await ProcessVideoAsync(path, videoId);
    }

    public async Task AddSubtitlesAsync(int videoId, IDictionary<string, Stream> subtitles)
    {
        foreach (var kv in subtitles)
        {
            var lang = kv.Key;
            var subStream = kv.Value;
            var subPath = await _storage.SaveSubtitleAsync(videoId, lang, subStream);
            await _subtitleRepository.AddAsync(new Subtitle
            {
                VideoId = videoId,
                Language = lang,
                FilePath = subPath
            });
        }
    }
}
