using System.Threading.Tasks;
using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Interfaces;

public interface IVideoRepository : IRepository<Video>
{
    Task<IPagedList<Video>> GetByChannelIdAsync(int channedlId, int page = 0, int pageSize = int.MaxValue);
}
