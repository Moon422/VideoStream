using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Infrastructure.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db;
    private readonly IPaginator _paginator;

    public BaseRepository(AppDbContext db, IPaginator paginator)
    {
        _db = db;
        _paginator = paginator;
    }

    public virtual async Task AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
        }

        return await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IPagedList<T>> GetAllAsync(int page = 0, int pageSize = int.MaxValue)
    {
        return await _paginator.PaginateAsync(_db.Set<T>().AsNoTracking().OrderBy(e => e.Id), page, pageSize);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _db.Set<T>().Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public virtual Task<bool> ExistsAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
        }

        return _db.Set<T>().AnyAsync(e => e.Id == id);
    }
}
