using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoStream.Application.Events;

namespace VideoStream.Infrastructure.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IServiceScopeFactory scopeFactory, ILogger<EventPublisher> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        using var scope = _scopeFactory.CreateScope();
        var consumers = scope.ServiceProvider.GetServices<IConsumer<TEvent>>();

        foreach (var consumer in consumers)
        {
            try
            {
                await consumer.HandleEventAsync(@event);
                if (@event is IStopProcessingEvent { StopProcessing: true }) break;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error handling event {EventType} by {Consumer}", typeof(TEvent).Name, consumer.GetType().Name);
            }
        }
    }
}
