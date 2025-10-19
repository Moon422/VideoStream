using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure;

public class Paginator : IPaginator
{
    private readonly IPagedListFactory _factory;

    public Paginator(IPagedListFactory factory)
    {
        _factory = factory;
    }

    public async Task<IPagedList<T>> PaginateAsync<T>(IQueryable<T> query, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var count = await query.CountAsync();
        var items = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        return _factory.Create(items, count, pageIndex, pageSize);
    }
}