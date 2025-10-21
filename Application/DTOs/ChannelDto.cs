using System;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class ChannelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public static class ChannelToChannelDtoHelper
{
    public static ChannelDto ToChannelDto(this Channel channel)
    {
        return new ChannelDto
        {
            Name = channel.Name,
            Description = channel.Description,
            CreatedByUserId = channel.CreatedByUserId,
            CreatedOn = channel.CreatedOn,
            ModifiedOn = channel.ModifiedOn
        };
    }
}