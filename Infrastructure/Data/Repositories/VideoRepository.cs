using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Application.Events;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Infrastructure.Data.Repositories;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    private readonly IPaginator _paginator;

    public VideoRepository(AppDbContext db,
        IPaginator paginator,
        IEventPublisher eventPublisher) : base(db,
            paginator,
            eventPublisher)
    {
        _paginator = paginator;
    }

    public async Task<IPagedList<Video>> GetByChannelIdAsync(int channelId, int page = 0, int pageSize = int.MaxValue)
    {
        if (channelId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(channelId), "Channel Id must be positive.");
        }

        var query = _db.Videos.AsNoTracking().Where(v => v.ChannelId == channelId).OrderByDescending(v => v.CreatedOn);
        return await _paginator.PaginateAsync(query, page, pageSize);
    }
}
