using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Exceptions;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadVideoUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly IWorkContext _workContext;

    public UploadVideoUseCase(IVideoRepository videoRepository,
        IChannelRepository channelRepository,
        IVideoProcessingService videoProcessingService,
        IWorkContext workContext)
    {
        _videoProcessingService = videoProcessingService;
        _videoRepository = videoRepository;
        _channelRepository = channelRepository;
        _workContext = workContext;
    }

    public async Task ExecuteAsync(UploadVideoDto request)
    {
        var currentUser = await _workContext.GetCurrentUserAsync()
            ?? throw new UserNotFoundException();

        var video = await _videoRepository.GetByIdAsync(request.VideoId)
            ?? throw new EntityNotFoundException(typeof(Video), request.VideoId);

        var channel = await _channelRepository.GetByIdAsync(video.ChannelId)
            ?? throw new EntityNotFoundException(typeof(Channel), video.ChannelId);

        if (currentUser.Id != channel.CreatedByUserId)
            throw new InvalidOperationException("You do not have video upload permission to the channel.");

        await _videoProcessingService.EnqueueProcessingAsync(video.Id, request.VideoStream);
    }
}