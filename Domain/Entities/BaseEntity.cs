using System;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}

public interface ISoftDeleted
{
    public bool Deleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}

public interface ICreationLogged
{
    public DateTime CreatedOn { get; set; }
}

public interface IModificationLogged
{
    public DateTime? ModifiedOn { get; set; }
}
