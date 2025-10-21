using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Videos;

public record VideoModel : BaseEntityModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public string HlsMasterPlaylistPath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
}

public static class VideoDtoToVideoModelHelper
{
    public static VideoModel ToVideoModel(this VideoDto videoDto)
    {
        return new VideoModel
        {
            Id = videoDto.Id,
            Title = videoDto.Title,
            Description = videoDto.Description,
            Tags = videoDto.Tags,
            ChannelId = videoDto.ChannelId,
            HlsMasterPlaylistPath = videoDto.HlsMasterPlaylistPath,
            ThumbnailPath = videoDto.ThumbnailPath
        };
    }
}