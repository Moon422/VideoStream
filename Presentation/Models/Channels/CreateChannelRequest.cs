using System.ComponentModel.DataAnnotations;

namespace VideoStream.Presentation.Models.Channels;

public record CreateChannelRequest : BaseModel
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }
}
