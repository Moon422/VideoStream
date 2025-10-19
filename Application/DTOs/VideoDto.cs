using System;
using System.ComponentModel.DataAnnotations;

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
