using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class CreateChannelDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }

    public Channel ToChannel()
    {
        return new Channel
        {
            Name = Name,
            Description = Description,
            CreatedByUserId = CreatedByUserId,
        };
    }
}