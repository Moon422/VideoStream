using System.IO;
using System.Threading.Tasks;

namespace VideoStream.Application.Interfaces;

public interface ILocalFileStorageService
{
    Task<string> SaveSubtitleAsync(int videoId, string language, Stream content);
    Task<string> SaveVideoAsync(int videoId, Stream content, string fileName);
}