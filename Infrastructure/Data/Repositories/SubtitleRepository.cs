using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Infrastructure.Data.Repositories;

public class SubtitleRepository : BaseRepository<Subtitle>, ISubtitleRepository
{
    private readonly IPaginator _paginator;

    public SubtitleRepository(AppDbContext db, IPaginator paginator) : base(db, paginator)
    {
        _paginator = paginator;
    }

    public async Task<Subtitle?> GetSubtitleAsync(int videoId, string language)
        => await _db.Subtitles.AsNoTracking().FirstOrDefaultAsync(s => s.VideoId == videoId && s.Language == language);

    public async Task<IPagedList<Subtitle>> GetVideoSubtitlesAsync(int videoId, int page = 0, int pageSize = int.MaxValue)
    {
        var query = _db.Subtitles.AsNoTracking().Where(s => s.VideoId == videoId).OrderBy(s => s.Language);
        return await _paginator.PaginateAsync(query, page, pageSize);
    }
}
