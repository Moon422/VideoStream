using System.Threading.Tasks;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Pagination;

namespace VideoStream.Domain.Interfaces;

public interface ISubtitleRepository : IRepository<Subtitle>
{
    Task<Subtitle?> GetSubtitleAsync(int videoId, string language);
    Task<IPagedList<Subtitle>> GetVideoSubtitlesAsync(int videoId, int page = 0, int pageSize = int.MaxValue);
}