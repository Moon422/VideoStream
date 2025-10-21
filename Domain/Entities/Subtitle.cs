using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public class Subtitle : BaseEntity
{
    [Required]
    public int VideoId { get; set; }

    [Required, MaxLength(16)]
    public required string Language { get; set; }

    [Required, MaxLength(1024)]
    public required string FilePath { get; set; }
}
