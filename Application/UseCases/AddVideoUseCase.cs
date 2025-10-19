using System;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class AddVideoUseCase
{
    private readonly IVideoRepository _videoRepository;

    public AddVideoUseCase(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<VideoDto> ExecuteAsync(VideoCreateDto request)
    {
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
