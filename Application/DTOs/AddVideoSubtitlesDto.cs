using System.Collections.Generic;
using System.IO;

namespace VideoStream.Application.DTOs;

public class AddVideoSubtitlesDto
{
    public int VideoId { get; set; }
    public IDictionary<string, Stream> Subtitles { get; set; }
}