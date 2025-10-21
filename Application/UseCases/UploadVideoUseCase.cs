using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadVideoUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IVideoProcessingService _videoProcessingService;

    public UploadVideoUseCase(IVideoRepository videoRepository,
        IVideoProcessingService videoProcessingService)
    {
        _videoProcessingService = videoProcessingService;
        _videoRepository = videoRepository;
    }

    public async Task ExecuteAsync(UploadVideoDto request)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId);
        if (video is null)
        {
            throw new ArgumentNullException("Video not found.");
        }

        await _videoProcessingService.EnqueueProcessingAsync(video.Id, request.VideoStream);
    }
}