using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VideoStream.Application.Events;

namespace VideoStream.Infrastructure.Events;

public partial class EventPublisher : IEventPublisher
{
    private readonly IServiceScope _serviceScope;

    public EventPublisher(IServiceScopeFactory scopeFactory)
    {
        _serviceScope = scopeFactory.CreateScope();
    }

    #region Methods

    public virtual async Task PublishAsync<TEvent>(TEvent @event)
    {
        var consumers = _serviceScope.ServiceProvider.GetServices<IConsumer<TEvent>>();

        foreach (var consumer in consumers)
            try
            {
                await consumer.HandleEventAsync(@event);

                if (@event is IStopProcessingEvent { StopProcessing: true })
                    break;
            }
            catch // (Exception exception)
            {
                // try
                // {
                //     var logger = EngineContext.Current.Resolve<ILogger>();
                //     if (logger == null)
                //         return;

                //     await logger.ErrorAsync(exception.Message, exception);
                // }
                // catch
                // {
                //     // ignored
                // }
            }
    }

    public virtual void Publish<TEvent>(TEvent @event)
    {
        var consumers = _serviceScope.ServiceProvider.GetServices<IConsumer<TEvent>>().ToList();

        foreach (var consumer in consumers)
            try
            {
                consumer.HandleEventAsync(@event).Wait();

                if (@event is IStopProcessingEvent { StopProcessing: true })
                    break;
            }
            catch // (Exception exception)
            {
                // try
                // {
                //     var logger = EngineContext.Current.Resolve<ILogger>();
                //     if (logger == null)
                //         return;

                //     logger.Error(exception.Message, exception);
                // }
                // catch
                // {
                //     // ignored
                // }
            }
    }

    #endregion
}