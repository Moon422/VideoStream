using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class AddSubtitlesUseCase
{
    private readonly IVideoProcessingService _videoProcessingService;

    public AddSubtitlesUseCase(IVideoProcessingService videoProcessingService)
    {
        _videoProcessingService = videoProcessingService;
    }

    public async Task ExecuteAsync(AddVideoSubtitlesDto request)
    {
        await _videoProcessingService.AddSubtitlesAsync(request.VideoId, request.Subtitles);
    }
}