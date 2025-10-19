using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VideoStream.Domain.Interfaces;

public interface IVideoProcessingService
{
    Task EnqueueProcessingAsync(int videoId, Stream videoStream);
    Task AddSubtitlesAsync(int videoId, IDictionary<string, Stream> subtitles);
}
