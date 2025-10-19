using System.Collections.Generic;
using System.Threading.Tasks;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Pagination;

namespace VideoStream.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<IPagedList<T>> GetAllAsync(int page = 0, int pageSize = int.MaxValue);
    Task UpdateAsync(T entity);
    Task<bool> ExistsAsync(int id);
}
