using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class UploadSubtitlesUseCase
{
    private readonly ISubtitleRepository _subtitleRepository;
    private readonly ILocalFileStorageService _localFileStorageService;

    public UploadSubtitlesUseCase(ISubtitleRepository subtitleRepository,
        ILocalFileStorageService localFileStorageService)
    {
        _subtitleRepository = subtitleRepository;
        _localFileStorageService = localFileStorageService;
    }

    public async Task ExecuteAsync(UploadVideoSubtitlesDto request)
    {
        foreach (var subtitle in await request.ToSubtitleList(_localFileStorageService))
        {
            await _subtitleRepository.AddAsync(subtitle);
        }
    }
}