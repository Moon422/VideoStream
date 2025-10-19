using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadVideoUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly ISearchService _searchService;

    public UploadVideoUseCase(
        IVideoRepository videoRepository,
        IVideoProcessingService videoProcessingService,
        ISearchService searchService)
    {
        _videoRepository = videoRepository;
        _videoProcessingService = videoProcessingService;
        _searchService = searchService;
    }

    public async Task<VideoDto> ExecuteAsync(VideoUploadDto request)
    {
        if (request.VideoStream == null)
            throw new ArgumentException("Video stream is required", nameof(request.VideoStream));

        var video = new Video
        {
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags,
            ChannelId = request.ChannelId,
            Status = VideoStatus.Pending,
            CreatedOn = DateTime.UtcNow
        };

        await _videoRepository.AddAsync(video);

        await _videoProcessingService.EnqueueProcessingAsync(video.Id, request.VideoStream, request.Subtitles);

        await _searchService.IndexVideoAsync(video);

        return new VideoDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            Tags = video.Tags,
            ChannelId = video.ChannelId,
            FilePath = video.FilePath,
            HlsMasterPlaylistPath = video.HlsMasterPlaylistPath,
            ThumbnailPath = video.ThumbnailPath,
            Status = video.Status.ToString(),
            CreatedOn = video.CreatedOn
        };
    }
}
