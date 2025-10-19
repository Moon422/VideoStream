using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VideoStream.Application;
using VideoStream.Application.Interfaces;
using VideoStream.Infrastructure;
using VideoStream.Infrastructure.Data;
using VideoStream.Presentation.Caching;

namespace VideoStream.Presentation;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddSwaggerGen();

        services.AddMemoryCache();

        services.AddScoped<ICacheManager, CacheManager>();

        // SQLite DbContext
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        // Application services
        services.AddApplication();

        // Infrastructure services
        services.AddInfrastructure(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        // If you add authentication, call app.UseAuthentication() before UseAuthorization()
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            if (env.IsDevelopment())
            {
                endpoints.MapOpenApi();
            }
            endpoints.MapControllers();
        });
    }
}
