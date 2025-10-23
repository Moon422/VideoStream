using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Exceptions;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class CreateVideoUseCase
{
    private readonly IChannelRepository _channelRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IWorkContext _workContext;

    public CreateVideoUseCase(IChannelRepository channelRepository,
        IVideoRepository videoRepository,
        IWorkContext workContext)
    {
        _channelRepository = channelRepository;
        _videoRepository = videoRepository;
        _workContext = workContext;
    }

    public async Task<VideoDto> ExecuteAsync(CreateVideoInformationDto request)
    {
        var currentUser = await _workContext.GetCurrentUserAsync()
            ?? throw new UserNotFoundException();

        var channel = await _channelRepository.GetByIdAsync(request.ChannelId)
            ?? throw new EntityNotFoundException(typeof(Channel), request.ChannelId);

        if (currentUser.Id != channel.CreatedByUserId)
            throw new InvalidOperationException("You do not have video upload permission to the channel.");

        var video = await _videoRepository.AddAsync(request.ToVideo());
        return video.ToVideoDto();
    }
}
