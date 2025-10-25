using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoStream.Application.Events;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Interfaces;
using VideoStream.Infrastructure.Data;
using VideoStream.Infrastructure.Data.Repositories;
using VideoStream.Infrastructure.Events;
using VideoStream.Infrastructure.Pagination;
using VideoStream.Infrastructure.Services;
using VideoStream.Infrastructure.Storage;
using System.IO;

namespace VideoStream.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("mysql");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException();

            var version = ServerVersion.AutoDetect(connectionString) ??
                throw new InvalidOperationException();

            options.UseMySql(connectionString, version);
        });

        // Pagination
        services.AddScoped<IPagedListFactory, PagedListFactory>();
        services.AddScoped<IPaginator, Paginator>();

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<ISubtitleRepository, SubtitleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Events
        services.AddScoped<IEventPublisher, EventPublisher>();

        // Storage
        services.AddSingleton<IFileStorageService, LocalFileStorageService>(sp =>
        {
            var storageRoot = configuration["Storage:Root"] ?? Path.Combine(AppContext.BaseDirectory, "storage");
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LocalFileStorageService>>();
            return new LocalFileStorageService(logger, storageRoot);
        });

        // services.AddAzureClients(clientBuilder =>
        // {
        //     clientBuilder.AddBlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
        // });

        // services.AddSingleton<IFileStorageService, AzureFileStorageService>();

        // Services
        services.AddScoped<IVideoProcessingService, VideoProcessingService>();
        services.AddScoped<ISearchService, AlgoliaSearchService>();

        return services;
    }
}