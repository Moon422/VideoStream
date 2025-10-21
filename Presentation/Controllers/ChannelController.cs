using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.UseCases;
using VideoStream.Domain.Entities;
using VideoStream.Presentation.Models.Channels;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    private readonly CreateChannelUseCase _createChannelUseCase;
    private readonly GetChannelByIdUseCase _getChannelByIdUseCase;

    public ChannelController(CreateChannelUseCase createChannelUseCase,
        GetChannelByIdUseCase getChannelByIdUseCase)
    {
        _createChannelUseCase = createChannelUseCase;
        _getChannelByIdUseCase = getChannelByIdUseCase;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Channel>> GetById(int id)
    {
        var channelDto = await _getChannelByIdUseCase.ExecuteAsync(id);
        if (channelDto is null)
            return NotFound();

        return Ok(channelDto.ToChannelModel());
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateChannelRequest req)
    {
        var channelDto = await _createChannelUseCase.ExecuteAsync(req.ToCreateChannelDto());
        return CreatedAtAction(nameof(GetById), new { id = channelDto.Id }, channelDto.ToChannelModel());
    }
}
