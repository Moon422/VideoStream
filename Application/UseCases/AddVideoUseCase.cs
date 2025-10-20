using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class AddVideoUseCase
{
    private readonly IVideoRepository _videoRepository;

    public AddVideoUseCase(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<VideoDto> ExecuteAsync(AddVideoInformationDto request)
    {
        var video = await _videoRepository.AddAsync(request.ToVideo());
        return video.ToVideoDto();
    }
}
