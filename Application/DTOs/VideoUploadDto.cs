using System.IO;

namespace VideoStream.Application.DTOs;

public class VideoUploadDto
{
    public int VideoId { get; set; }
    public Stream VideoStream { get; set; }
}
