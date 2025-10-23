using System.Threading.Tasks;
using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, bool skipDeleted = true);
    Task<User?> GetByUsernameAsync(string username, bool skipDeleted = true);
    Task<bool> IsAdminAsync(int userId);
}