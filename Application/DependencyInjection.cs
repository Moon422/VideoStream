using Microsoft.Extensions.DependencyInjection;
using VideoStream.Application.UseCases;

namespace VideoStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Use cases
        services.AddScoped<AddVideoUseCase>();
        services.AddScoped<AddSubtitlesUseCase>();
        services.AddScoped<UploadVideoUseCase>();
        services.AddScoped<SearchVideosUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<CreateChannelUseCase>();
        services.AddScoped<GetChannelByIdUseCase>();

        // TODO: register mappers/validators/behaviors here when added
        return services;
    }
}