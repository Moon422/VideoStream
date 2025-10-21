using System.IO;

namespace VideoStream.Application.DTOs;

public class UploadVideoDto
{
    public int VideoId { get; set; }
    public required Stream VideoStream { get; set; }
}
