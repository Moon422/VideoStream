using Microsoft.Extensions.DependencyInjection;
using VideoStream.Application.UseCases;

namespace VideoStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Use cases
        services.AddScoped<CreateVideoUseCase>();
        services.AddScoped<GetVideoByIdUseCase>();
        services.AddScoped<UploadSubtitlesUseCase>();
        services.AddScoped<UploadVideoUseCase>();
        services.AddScoped<SearchVideosUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<GetUserByEmailUseCase>();
        services.AddScoped<GetUserByUsernameUseCase>();
        services.AddScoped<CreateChannelUseCase>();
        services.AddScoped<GetChannelByIdUseCase>();
        services.AddScoped<GetVideosByChannelIdUseCase>();

        // TODO: register mappers/validators/behaviors here when added
        return services;
    }
}