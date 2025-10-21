using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Videos;

public record VideoOverviewModel : BaseEntityModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public string ThumbnailPath { get; set; } = string.Empty;
}

public static class VideoDtoToVideoOverviewModelHelper
{
    public static VideoOverviewModel ToVideoOverviewModel(this VideoDto videoDto)
    {
        return new VideoOverviewModel
        {
            Id = videoDto.Id,
            Title = videoDto.Title,
            Description = videoDto.Description,
            Tags = videoDto.Tags,
            ChannelId = videoDto.ChannelId,
            ThumbnailPath = videoDto.ThumbnailPath
        };
    }
}