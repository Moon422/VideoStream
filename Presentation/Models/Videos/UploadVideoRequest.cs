using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Videos;

public class UploadVideoRequest
{
    public string Title { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }

    public AddVideoInformationDto ToAddVideoInformationDto()
    {
        return new AddVideoInformationDto
        {
            Title = Title,
            ChannelId = ChannelId,
            Description = Description ?? string.Empty,
            Tags = Tags ?? string.Empty,
        };
    }
}
