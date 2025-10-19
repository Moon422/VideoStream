using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Events;

public partial class EntityDeletedEvent<T> where T : BaseEntity
{
    public EntityDeletedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
