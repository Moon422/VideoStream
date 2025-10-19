using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Application.Caching;
using VideoStream.Application.Events;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;
using VideoStream.Infrastructure.Events;

namespace VideoStream.Infrastructure.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db;
    private readonly IPaginator _paginator;
    private readonly IEventPublisher _eventPublisher;
    private readonly ICacheManager _cacheManager;

    public BaseRepository(AppDbContext db,
        IPaginator paginator,
        IEventPublisher eventPublisher,
        ICacheManager cacheManager)
    {
        _db = db;
        _paginator = paginator;
        _eventPublisher = eventPublisher;
        _cacheManager = cacheManager;
    }

    protected virtual IQueryable<T> AddDeletedFilter(IQueryable<T> query, in bool includeDeleted)
    {
        if (includeDeleted)
            return query;

        if (typeof(T).GetInterface(nameof(ISoftDeleted)) == null)
            return query;

        return query.OfType<ISoftDeleted>().Where(entry => !entry.Deleted).OfType<T>();
    }

    public virtual async Task AddAsync(T entity, bool publishEvent = true)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();

        if (publishEvent)
        {
            await _eventPublisher.EntityInsertedAsync(entity);
        }
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
        }

        var cacheKey = _cacheManager.PrepareCacheKey(EntityCacheDefaults<T>.ByIdCacheKey, id);
        return await _cacheManager.GetAsync(cacheKey, async () => await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id));
    }

    public virtual async Task<IPagedList<T>> GetAllAsync(int page = 0, int pageSize = int.MaxValue)
    {
        var cacheKey = _cacheManager.PrepareCacheKey(EntityCacheDefaults<T>.AllPagedCacheKey, page, pageSize);
        return await _cacheManager.GetAsync(cacheKey, async () => await _paginator.PaginateAsync(_db.Set<T>().AsNoTracking().OrderBy(e => e.Id), page, pageSize));
    }

    public virtual async Task UpdateAsync(T entity, bool publishEvent = true)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _db.Set<T>().Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        if (publishEvent)
        {
            await _eventPublisher.EntityUpdatedAsync(entity);
        }
    }

    // public virtual Task<bool> ExistsAsync(int id)
    // {
    //     if (id <= 0)
    //     {
    //         throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
    //     }

    //     if (typeof(T).GetInterface(nameof(ISoftDeleted)) is not null)
    //         return _db.Set<T>().AnyAsync(e => e.Id == id && e.);

    //     return _db.Set<T>().AnyAsync(e => e.Id == id);
    // }
}
