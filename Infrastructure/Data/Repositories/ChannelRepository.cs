using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Data.Repositories;

public class ChannelRepository : BaseRepository<Channel>, IChannelRepository
{
    public ChannelRepository(AppDbContext db, IPaginator paginator) : base(db, paginator) { }
}
