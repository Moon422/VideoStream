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
using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using System.Threading.Tasks;

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
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "video Stream API",
                Version = "v1"
            });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        services.AddAuthentication().AddJwtBearer(
            options =>
            {
                var secret = Configuration.GetSection("Secret").Value
                    ?? throw new InvalidOperationException("JWT secret missing.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? string.Empty;

                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Cookies["jwt"];
                        }

                        context.Token = token;
                        return Task.CompletedTask;
                    },
                };
            }
        );

        services.AddHttpContextAccessor();

        services.AddMemoryCache();

        services.AddScoped<ICacheManager, CacheManager>();

        // Application services
        services.AddApplication();

        // Infrastructure services
        services.AddInfrastructure(Configuration);

        services.AddScoped<IWorkContext, WorkContext>();
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

        app.UseAuthentication();
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
