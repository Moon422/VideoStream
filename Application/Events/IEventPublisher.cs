using System.Threading.Tasks;

namespace VideoStream.Application.Events;

public partial interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event);

    void Publish<TEvent>(TEvent @event);
}
