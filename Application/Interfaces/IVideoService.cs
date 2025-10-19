using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain;

namespace VideoStream.Application.Interfaces;

public interface IVideoService
{
    Task<VideoDto> UploadAsync(VideoUploadDto dto);
    Task<VideoDto?> GetByIdAsync(int id);
    Task<IPagedList<VideoDto>> GetByChannelAsync(int channelId, int page = 0, int pageSize = 50);
}
