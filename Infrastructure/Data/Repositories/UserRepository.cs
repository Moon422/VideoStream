using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Application.Events;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db,
        IPaginator paginator,
        IEventPublisher eventPublisher,
        ICacheManager cacheManager) : base(db,
            paginator,
            eventPublisher,
            cacheManager)
    { }

    public async Task<User?> GetByEmailAsync(string email, bool skipDeleted = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => (!skipDeleted || !u.Deleted) && u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username, bool skipDeleted = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => (!skipDeleted || !u.Deleted) && u.Username == username);
    }

    public async Task<bool> IsAdminAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), "User Id must be positive.");
        }

        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null || !user.IsAdmin)
            return false;

        return true;
    }
}
