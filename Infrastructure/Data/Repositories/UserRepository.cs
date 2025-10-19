using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Application.Events;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db,
        IPaginator paginator,
        IEventPublisher eventPublisher) : base(db,
            paginator,
            eventPublisher)
    { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
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
