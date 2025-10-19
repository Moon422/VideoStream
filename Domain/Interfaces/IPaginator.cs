using System.Linq;
using System.Threading.Tasks;

namespace VideoStream.Domain.Interfaces;

public interface IPaginator
{
    Task<IPagedList<T>> PaginateAsync<T>(IQueryable<T> query, int pageIndex = 0, int pageSize = int.MaxValue);
}