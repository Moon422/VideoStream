using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoStream.Application.Interfaces;

namespace VideoStream.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly string _root;

    public LocalFileStorageService(ILogger<LocalFileStorageService> logger, string root)
    {
        _logger = logger;
        _root = root;
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveVideoAsync(int videoId, string fileName, Stream content)
    {
        var dir = Path.Combine(_root, "videos", videoId.ToString());
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, fileName);
        using (var fs = File.Create(path))
        {
            await content.CopyToAsync(fs);
        }
        _logger.LogInformation("Saved video to {Path}", path);
        return path;
    }

    public async Task<string> SaveSubtitleAsync(int videoId, string language, Stream content)
    {
        var dir = Path.Combine(_root, "videos", videoId.ToString(), "subtitles");
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, $"{language}.vtt");
        using (var fs = File.Create(path))
        {
            await content.CopyToAsync(fs);
        }
        _logger.LogInformation("Saved subtitle {Lang} to {Path}", language, path);
        return path;
    }
}
