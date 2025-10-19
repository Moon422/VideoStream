using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelsController : ControllerBase
{
    private readonly IChannelRepository _channels;

    public ChannelsController(IChannelRepository channels)
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

    public record CreateChannelRequest(string Name, string? Description, int CreatedByUserId);

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateChannelRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest("Name is required");
        var channel = new Channel
        {
            Name = req.Name,
            Description = req.Description ?? string.Empty,
            CreatedByUserId = req.CreatedByUserId,
            CreatedOn = DateTime.UtcNow
        };
        await _channels.AddAsync(channel);
        return CreatedAtAction(nameof(GetById), new { id = channel.Id }, channel);
    }
}
