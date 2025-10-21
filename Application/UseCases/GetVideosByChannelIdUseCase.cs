using System.Linq;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Application.UseCases;

public class GetVideosByChannelIdUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IPagedListFactory _pagedListFactory;

    public GetVideosByChannelIdUseCase(IVideoRepository videoRepository, IPagedListFactory pagedListFactory)
    {
        _videoRepository = videoRepository;
        _pagedListFactory = pagedListFactory;
    }

    public async Task<IPagedList<VideoDto>> ExecuteAsync(int channelId, int page = 0, int pageSize = int.MaxValue)
    {
        var videos = await _videoRepository.GetByChannelIdAsync(channelId, page, pageSize);
        return _pagedListFactory.Create(
            videos.Select(v => v.ToVideoDto()).ToList(),
            videos.TotalItems,
            videos.PageIndex,
            videos.PageSize);
    }
}