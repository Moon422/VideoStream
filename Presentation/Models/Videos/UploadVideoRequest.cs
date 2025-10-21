using System.ComponentModel.DataAnnotations;
using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Videos;

public class CreateVideoRequest
{
    [Required, MaxLength(200)]
    public required string Title { get; set; }
    public int ChannelId { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }

    public CreateVideoInformationDto ToAddVideoInformationDto()
    {
        return new CreateVideoInformationDto
        {
            Title = Title,
            ChannelId = ChannelId,
            Description = Description ?? string.Empty,
            Tags = Tags ?? string.Empty,
        };
    }
}
