using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db, IPaginator paginator) : base(db, paginator) { }

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByUsernameAsync(string username)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

    public async Task IsAdminAsync(int userId)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || !user.IsAdmin)
            throw new UnauthorizedAccessException("Admin privileges required");
    }
}
