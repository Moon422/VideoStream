using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using VideoStream.Application.Interfaces;

namespace VideoStream.Infrastructure.Storage;

public class AzureFileStorageService : IFileStorageService
{
    private readonly ILogger<AzureFileStorageService> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public AzureFileStorageService(ILogger<AzureFileStorageService> logger,
        BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public Task<string> SaveSubtitleAsync(int videoId, string language, Stream content)
    {
        throw new System.NotImplementedException();
    }

    public Task<string> SaveVideoAsync(int videoId, Stream content, string fileName)
    {
        _blobServiceClient.
    }
}