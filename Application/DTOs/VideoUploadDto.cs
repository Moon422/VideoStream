using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Application.DTOs;

public class VideoUploadDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    // Comma-separated tags
    [MaxLength(1024)]
    public string Tags { get; set; } = string.Empty;

    [Required]
    public int ChannelId { get; set; }

    // Binary payloads
    [Required]
    public Stream VideoStream { get; set; } = Stream.Null;

    // key = language (e.g., "en", "fr"), value = subtitle file stream
    public Dictionary<string, Stream> Subtitles { get; set; } = new();
}
