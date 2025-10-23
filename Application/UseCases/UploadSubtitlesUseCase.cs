using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Exceptions;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadSubtitlesUseCase
{
    private readonly ISubtitleRepository _subtitleRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly ILocalFileStorageService _localFileStorageService;
    private readonly IWorkContext _workContext;

    public UploadSubtitlesUseCase(ISubtitleRepository subtitleRepository,
        IVideoRepository videoRepository,
        IChannelRepository channelRepository,
        ILocalFileStorageService localFileStorageService,
        IWorkContext workContext)
    {
        _subtitleRepository = subtitleRepository;
        _localFileStorageService = localFileStorageService;
        _videoRepository = videoRepository;
        _channelRepository = channelRepository;
        _workContext = workContext;
    }

    public async Task ExecuteAsync(UploadVideoSubtitlesDto request)
    {
        var currentUser = await _workContext.GetCurrentUserAsync()
            ?? throw new UserNotFoundException();

        var video = await _videoRepository.GetByIdAsync(request.VideoId)
            ?? throw new EntityNotFoundException(typeof(Video), request.VideoId);

        var channel = await _channelRepository.GetByIdAsync(video.ChannelId)
            ?? throw new EntityNotFoundException(typeof(Channel), video.ChannelId);

        if (currentUser.Id != channel.CreatedByUserId)
            throw new InvalidOperationException("You do not have video upload permission to the channel.");

        foreach (var subtitle in await request.ToSubtitleList(_localFileStorageService))
        {
            await _subtitleRepository.AddAsync(subtitle);
        }
    }
}