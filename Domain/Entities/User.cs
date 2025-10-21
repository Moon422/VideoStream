using System;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public class User : BaseEntity, ICreationLogged, IModificationLogged, ISoftDeleted
{
    [MaxLength(100)]
    public required string Firstname { get; set; }

    [MaxLength(100)]
    public required string Lastname { get; set; }

    [Required, MaxLength(50)]
    public required string Username { get; set; }

    [Required, MaxLength(256)]
    [EmailAddress]
    public required string Email { get; set; }

    public bool IsAdmin { get; set; }

    [Required, MaxLength(60)]
    public required string PasswordHash { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
