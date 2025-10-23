using System;

namespace VideoStream.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType, int entityId)
        : base($"Entity of type ${entityType.Name} with id: ${entityId} not found.")
    { }
}
