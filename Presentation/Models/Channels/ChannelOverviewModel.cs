using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Channels;

public record ChannelModel : BaseEntityModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
}

public static class ChannelDtoToChannelModelHelper
{
    public static ChannelModel ToChannelModel(this ChannelDto channelDto)
    {
        return new ChannelModel
        {
            Id = channelDto.Id,
            Name = channelDto.Name,
            Description = channelDto.Description,
            CreatedByUserId = channelDto.CreatedByUserId
        };
    }
}