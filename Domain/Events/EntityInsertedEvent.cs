using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Events;

public partial class EntityInsertedEvent<T> where T : BaseEntity
{
    public EntityInsertedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
