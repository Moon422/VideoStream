using VideoStream.Application.Events;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Data.Repositories;

public class ChannelRepository : BaseRepository<Channel>, IChannelRepository
{
    public ChannelRepository(AppDbContext db,
        IPaginator paginator,
        IEventPublisher eventPublisher,
        ICacheManager cacheManager) : base(db,
            paginator,
            eventPublisher,
            cacheManager)
    { }
}
