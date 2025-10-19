using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadVideoUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly ISearchService _searchService;

    public UploadVideoUseCase(IVideoRepository videoRepository,
        IVideoProcessingService videoProcessingService,
        ISearchService searchService)
    {
        _videoProcessingService = videoProcessingService;
        _searchService = searchService;
        _videoRepository = videoRepository;
    }

    public async Task ExecuteAsync(VideoUploadDto request)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId);
        if (video is null)
        {
            throw new ArgumentNullException("Video not found.");
        }

        await _videoProcessingService.EnqueueProcessingAsync(video.Id, request.VideoStream);
    }
}