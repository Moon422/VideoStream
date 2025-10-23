using System.IO;
using System.Threading.Tasks;

namespace VideoStream.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveSubtitleAsync(int videoId, string language, Stream content);
    Task<string> SaveVideoAsync(int videoId, string fileName, Stream content);
}