using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace VideoStream.Presentation.Models.Videos;

public class UploadVideoRequest
{
    public string Title { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
}
