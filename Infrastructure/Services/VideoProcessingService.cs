using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Infrastructure.Data;
using VideoStream.Infrastructure.Storage;

namespace VideoStream.Infrastructure.Services;

public class VideoProcessingService : IVideoProcessingService
{
    private readonly AppDbContext _db;
    private readonly ILogger<VideoProcessingService> _logger;
    private readonly LocalFileStorageService _storage;

    public VideoProcessingService(AppDbContext db, ILogger<VideoProcessingService> logger, LocalFileStorageService storage)
    {
        _db = db;
        _logger = logger;
        _storage = storage;
    }

    public async Task EnqueueProcessingAsync(int videoId, Stream videoStream, Dictionary<string, Stream> subtitles)
    {
        var video = await _db.Videos.FirstOrDefaultAsync(v => v.Id == videoId) ?? throw new InvalidOperationException($"Video {videoId} not found");
        // Save source video
        var srcName = $"{Guid.NewGuid():N}.mp4";
        var path = await _storage.SaveVideoAsync(videoId, videoStream, srcName);
        video.FilePath = path;
        video.Status = VideoStatus.Processing;
        await _db.SaveChangesAsync();

        // Save subtitles
        foreach (var kv in subtitles)
        {
            var lang = kv.Key;
            var subStream = kv.Value;
            var subPath = await _storage.SaveSubtitleAsync(videoId, lang, subStream);
            _db.Subtitles.Add(new Subtitle
            {
                VideoId = videoId,
                Language = lang,
                FilePath = subPath
            });
        }
        await _db.SaveChangesAsync();

        // In a real system, enqueue a background job to transcode and HLS package
        _logger.LogInformation("Queued processing for video {VideoId}", videoId);
    }
}
