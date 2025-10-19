using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VideoStream.Domain.Interfaces;

public interface IVideoProcessingService
{
    Task EnqueueProcessingAsync(int videoId, Stream videoStream, Dictionary<string, Stream> subtitles);
}
