using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace VideoStream.Presentation.Models.Videos;

public class UploadVideoRequest
{
    public string Title { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }

    // Main video file
    // public IFormFile File { get; set; } = default!;

    // Optional subtitles as multipart keys: Subtitles[en], Subtitles[fr], ...
    // public Dictionary<string, IFormFile>? Subtitles { get; set; }
}
