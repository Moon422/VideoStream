using System.ComponentModel.DataAnnotations;
using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Channels;

public record CreateChannelRequest : BaseModel
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }

    public CreateChannelDto ToCreateChannelDto()
    {
        return new CreateChannelDto
        {
            Name = Name,
            Description = Description ?? string.Empty,
            CreatedByUserId = CreatedByUserId
        };
    }
}
