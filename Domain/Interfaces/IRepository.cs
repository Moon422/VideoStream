using System.Threading.Tasks;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Pagination;

namespace VideoStream.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, bool publishEvent = true);
    Task<T?> GetByIdAsync(int id);
    Task<IPagedList<T>> GetAllAsync(int page = 0, int pageSize = int.MaxValue);
    Task UpdateAsync(T entity, bool publishEvent = true);
    // Task<bool> ExistsAsync(int id);
}
