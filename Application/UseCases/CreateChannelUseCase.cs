using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class CreateChannelUseCase
{
    private readonly IChannelRepository _channelRepository;

    public CreateChannelUseCase(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<ChannelDto> ExecuteAsync(CreateChannelDto request)
    {
        var channel = await _channelRepository.AddAsync(request.ToChannel());
        return channel.ToChannelDto();
    }
}
