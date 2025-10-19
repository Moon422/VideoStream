using Microsoft.Extensions.DependencyInjection;
using VideoStream.Application.UseCases;

namespace VideoStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Use cases
        services.AddScoped<UploadVideoUseCase>();
        services.AddScoped<SearchVideosUseCase>();

        // TODO: register mappers/validators/behaviors here when added
        return services;
    }
}