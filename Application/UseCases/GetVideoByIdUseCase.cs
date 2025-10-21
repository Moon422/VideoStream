using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

public class GetVideoByIdUseCase
{
    private readonly IVideoRepository _videoRepository;

    public GetVideoByIdUseCase(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<VideoDto?> ExecuteAsync(int videoId)
    {
        var entity = await _videoRepository.GetByIdAsync(videoId);
        if (entity == null)
            return null;

        return entity.ToVideoDto();
    }
}