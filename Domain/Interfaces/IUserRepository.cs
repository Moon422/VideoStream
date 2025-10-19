using System.Threading.Tasks;
using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> IsAdminAsync(int userId);
}