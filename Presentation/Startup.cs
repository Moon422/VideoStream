using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoStream.Application;
using VideoStream.Infrastructure;

namespace Presentation;

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
