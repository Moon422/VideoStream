using System.Threading.Tasks;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Pagination;

namespace VideoStream.Domain.Interfaces;

public interface IVideoRepository : IRepository<Video>
{
    Task<IPagedList<Video>> GetByChannelIdAsync(int channedlId, int page = 0, int pageSize = int.MaxValue);
}
