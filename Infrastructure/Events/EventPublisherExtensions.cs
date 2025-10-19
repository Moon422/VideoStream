using System.Threading.Tasks;
using VideoStream.Application.Events;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Events;

namespace VideoStream.Infrastructure.Events;

public static class EventPublisherExtensions
{
    public static async Task EntityInsertedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityInsertedEvent<T>(entity));
    }

    public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        eventPublisher.Publish(new EntityInsertedEvent<T>(entity));
    }

    public static async Task EntityUpdatedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityUpdatedEvent<T>(entity));
    }

    public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
    }

    public static async Task EntityDeletedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityDeletedEvent<T>(entity));
    }

    public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
    {
        eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
    }
}