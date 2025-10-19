using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public class Subtitle : BaseEntity
{
    [Required]
    public int VideoId { get; set; }

    [Required, MaxLength(16)]
    public string Language { get; set; } = string.Empty;

    [Required, MaxLength(1024)]
    public string FilePath { get; set; } = string.Empty;
}
