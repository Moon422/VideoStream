using System.ComponentModel.DataAnnotations;

namespace VideoStream.Presentation.Models.Videos;

public record VideoReadModel : BaseEntityModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tags { get; set; }
    public int ChannelId { get; set; }
    public string FilePath { get; set; }
    public string HlsMasterPlaylistPath { get; set; }
    public string ThumbnailPath { get; set; }
}