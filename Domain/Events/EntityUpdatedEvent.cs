using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Events;

public partial class EntityUpdatedEvent<T> where T : BaseEntity
{
    public EntityUpdatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}