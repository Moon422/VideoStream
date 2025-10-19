using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;
using VideoStream.Infrastructure.Pagination;

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
        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public virtual async Task<IPagedList<T>> GetAllAsync(int page = 0, int pageSize = int.MaxValue)
        => await _paginator.PaginateAsync(_db.Set<T>().AsNoTracking().OrderBy(e => e.Id), page, pageSize);

    public virtual async Task UpdateAsync(T entity)
    {
        _db.Set<T>().Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public virtual Task<bool> ExistsAsync(int id)
        => _db.Set<T>().AnyAsync(e => e.Id == id);
}
