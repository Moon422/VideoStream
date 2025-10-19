using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Infrastructure.Data.Repositories;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    private readonly IPaginator _paginator;

    public VideoRepository(AppDbContext db, IPaginator paginator) : base(db, paginator)
    {
        _paginator = paginator;
    }

    public async Task<IPagedList<Video>> GetByChannelIdAsync(int channedlId, int page = 0, int pageSize = int.MaxValue)
    {
        var query = _db.Videos.AsNoTracking().Where(v => v.ChannelId == channedlId).OrderByDescending(v => v.CreatedOn);
        return await _paginator.PaginateAsync(query, page, pageSize);
    }
}
