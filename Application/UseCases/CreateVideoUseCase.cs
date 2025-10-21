using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class CreateVideoUseCase
{
    private readonly IVideoRepository _videoRepository;

    public CreateVideoUseCase(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<VideoDto> ExecuteAsync(CreateVideoInformationDto request)
    {
        var video = await _videoRepository.AddAsync(request.ToVideo());
        return video.ToVideoDto();
    }
}
