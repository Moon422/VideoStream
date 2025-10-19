using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoStream.Domain.Entities;

public class Channel : BaseEntity, ICreationLogged, IModificationLogged, ISoftDeleted
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public int CreatedByUserId { get; set; }

    [InverseProperty(nameof(Video.Channel))]
    public IList<Video> Videos { get; set; } = [];

    [Required]
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
