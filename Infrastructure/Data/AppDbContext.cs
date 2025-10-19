using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoStream.Domain.Entities;

namespace VideoStream.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Subtitle> Subtitles => Set<Subtitle>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Video>()
            .HasOne(v => v.Channel)
            .WithMany(c => c.Videos)
            .HasForeignKey(v => v.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Subtitle>()
            .HasOne<Video>()
            .WithMany(v => v.Subtitles)
            .HasForeignKey(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Soft-delete query filters
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<Channel>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<Video>().HasQueryFilter(e => !e.Deleted);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var utcNow = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ICreationLogged cl && entry.State == EntityState.Added)
            {
                cl.CreatedOn = utcNow;
            }
            if (entry.Entity is IModificationLogged ml && (entry.State == EntityState.Modified))
            {
                ml.ModifiedOn = utcNow;
            }
        }
    }
}
