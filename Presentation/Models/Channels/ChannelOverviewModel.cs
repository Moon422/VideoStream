namespace VideoStream.Presentation.Models.Channels;

public record ChannelOverviewModel : BaseEntityModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
}