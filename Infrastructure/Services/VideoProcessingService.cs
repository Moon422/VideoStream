using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Extensions.Logging;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Services;

public class VideoProcessingService : IVideoProcessingService
{
    private readonly IVideoRepository _videoRepository;
    private readonly ILogger<VideoProcessingService> _logger;
    private readonly ILocalFileStorageService _storage;

    private readonly int[] _targetHeights = [360, 480, 720, 1080];

    public VideoProcessingService(IVideoRepository videoRepository,
        ILogger<VideoProcessingService> logger,
        ILocalFileStorageService storage)
    {
        _videoRepository = videoRepository;
        _logger = logger;
        _storage = storage;
    }

    private async Task<string?> ResizeAsync(string filepath, int width, int height, int targetHeight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filepath);

        if (targetHeight > height) return null;

        _logger.LogInformation("Resizing video file: {0} to {1}p", Path.GetFileName(filepath), targetHeight);

        int targetWidth = (int)((float)width / height * targetHeight);
        if (targetWidth % 2 != 0)
            targetWidth++;

        var directory = Path.GetDirectoryName(filepath);
        var fileExt = Path.GetExtension(filepath);
        var targetPath = Path.Join(directory, $"{targetWidth}_{targetHeight}.{fileExt.TrimStart('.')}");

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

    private async Task<string?> SegmentVideoToHlsAsync(string? filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath)) return null;

        _logger.LogInformation("Segmenting video file: {0}", Path.GetFileName(filepath));

        var filename = Path.GetFileNameWithoutExtension(filepath);
        var outputDirectory = Path.Join(Path.GetDirectoryName(filepath), filename);
        Directory.CreateDirectory(outputDirectory);
        var indexPlaylistPath = Path.Join(outputDirectory, $"index.m3u8");

        await FFMpegArguments.FromFileInput(filepath)
            .OutputToFile(indexPlaylistPath, true, options => options
                .ForceFormat("hls")
                .WithCustomArgument("-hls_time 10")
                .WithCustomArgument("-hls_list_size 0")
                .WithCopyCodec()
            ).ProcessAsynchronously();

        _logger.LogInformation("Segmented video file: {0}", Path.GetFileName(filepath));

        return indexPlaylistPath;
    }

    private async Task<string?> ProcessVideoAsync(string filepath)
    {
        _logger.LogInformation("Processing video file: {0}", Path.GetFileName(filepath));

        var mediaInfo = await FFProbe.AnalyseAsync(filepath);
        var width = mediaInfo.PrimaryVideoStream?.Width ?? 0;
        var height = mediaInfo.PrimaryVideoStream?.Height ?? 0;

        if (width < 0 || height < 0)
        {
            return null;
        }

        var masterPlaylistContent = new StringBuilder();
        masterPlaylistContent.AppendLine("#EXTM3U");

        foreach (var targetHeight in _targetHeights)
        {
            var resizedFilePath = await ResizeAsync(filepath, width, height, targetHeight);
            if (string.IsNullOrWhiteSpace(resizedFilePath))
                continue;

            var segmentedIndexFilepath = await SegmentVideoToHlsAsync(resizedFilePath);
            if (string.IsNullOrWhiteSpace(segmentedIndexFilepath))
                continue;

            var resizedMediaInfo = await FFProbe.AnalyseAsync(resizedFilePath);
            var bitrate = resizedMediaInfo.PrimaryVideoStream?.BitRate ?? (long)resizedMediaInfo.Format.BitRate;

            masterPlaylistContent.AppendLine($"#EXT-X-STREAM-INF:BANDWIDTH={bitrate},RESOLUTION={width}x{height}");
            masterPlaylistContent.AppendLine(segmentedIndexFilepath);
        }

        var outputDirectory = Path.GetDirectoryName(filepath);
        var masterPlaylistPath = Path.Join(outputDirectory, "master.m3u8");
        await File.WriteAllTextAsync(masterPlaylistPath, masterPlaylistContent.ToString());

        _logger.LogInformation("Processing video file compplete: {0}", Path.GetFileName(filepath));

        return masterPlaylistPath;
    }

    public async Task EnqueueProcessingAsync(int videoId, Stream videoStream)
    {
        var video = await _videoRepository.GetByIdAsync(videoId)
            ?? throw new InvalidOperationException($"Video {videoId} not found");

        // Save source video
        var srcName = "original.mp4";
        var path = await _storage.SaveVideoAsync(videoId, videoStream, srcName);
        video.FileName = srcName;
        video.FilePath = path;
        video.Status = VideoStatus.Processing;
        await _videoRepository.UpdateAsync(video);

        // In a real system, enqueue a background job to transcode and HLS package
        _logger.LogInformation("Queued processing for video {VideoId}", videoId);

        var masterPlaylistPath = await ProcessVideoAsync(path);
        if (!string.IsNullOrWhiteSpace(masterPlaylistPath))
        {
            video.HlsMasterPlaylistPath = masterPlaylistPath;
            video.Status = VideoStatus.Ready;
            await _videoRepository.UpdateAsync(video);
        }
    }
}
