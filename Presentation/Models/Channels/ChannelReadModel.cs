namespace VideoStream.Presentation.Models.Channels;

public record ChannelReadModel : BaseEntityModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
}