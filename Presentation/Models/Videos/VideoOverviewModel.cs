using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Videos;

public record VideoOverviewModel : BaseEntityModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tags { get; set; }
    public int ChannelId { get; set; }
    public string ThumbnailPath { get; set; }
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