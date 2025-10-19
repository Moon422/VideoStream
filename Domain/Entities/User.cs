using System;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public class User : BaseEntity, ICreationLogged, IModificationLogged, ISoftDeleted
{
    [MaxLength(100)]
    public string Firstname { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Lastname { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    [Required, MaxLength(512)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
