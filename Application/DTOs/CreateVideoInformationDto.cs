using System.ComponentModel.DataAnnotations;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class CreateVideoInformationDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    // Comma-separated tags
    [MaxLength(1024)]
    public string Tags { get; set; } = string.Empty;

    [Required]
    public int ChannelId { get; set; }

    public Video ToVideo()
    {
        return new Video
        {
            Title = Title,
            Description = Description,
            Tags = Tags,
            ChannelId = ChannelId
        };
    }
}
