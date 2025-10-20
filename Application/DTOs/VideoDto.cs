using System;
using System.ComponentModel.DataAnnotations;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class VideoDto
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string Tags { get; set; } = string.Empty;

    public int ChannelId { get; set; }

    public string FilePath { get; set; } = string.Empty;
    public string HlsMasterPlaylistPath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
}

public static class VideoToVideoDtoHelper
{
    public static VideoDto ToVideoDto(this Video video)
    {
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