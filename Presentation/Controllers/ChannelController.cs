using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Presentation.Models.Channels;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    private readonly IChannelRepository _channels;

    public ChannelController(IChannelRepository channels)
    {
        _channels = channels;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Channel>> GetById(int id)
    {
        var entity = await _channels.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateChannelRequest req)
    {
        var channel = new Channel
        {
            Name = req.Name,
            Description = req.Description ?? string.Empty,
            CreatedByUserId = req.CreatedByUserId,
            CreatedOn = DateTime.UtcNow
        };
        await _channels.AddAsync(channel);

        return CreatedAtAction(nameof(GetById), new ChannelReadModel
        {
            Id = channel.Id,
            Name = channel.Name,
            Description = channel.Description,
            CreatedByUserId = channel.CreatedByUserId
        }, channel);
    }
}
