using System;
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

    public async Task<string> SaveSubtitleAsync(int videoId, string language, Stream content)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
            var blobName = $"{videoId}/subtitles/{language}.vtt";
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload subtitle {VideoId}/subtitles/{Language}.vtt", videoId, language);
            return "";
        }
    }

    public async Task<string> SaveVideoAsync(int videoId, string fileName, Stream content)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
            var blobName = $"{videoId}/{fileName}";
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload video {VideoId}/{FileName}", videoId, fileName);
            return "";
        }
    }
}