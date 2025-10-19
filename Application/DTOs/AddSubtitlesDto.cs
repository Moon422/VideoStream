using System.Collections.Generic;
using System.IO;

namespace VideoStream.Application.DTOs;

public class AddSubtitlesDto
{
    public int VideoId { get; set; }
    public IDictionary<string, Stream> subtitles { get; set; }
}