using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class GetChannelByIdUseCase
{
    private readonly IChannelRepository _channelRepository;

    public GetChannelByIdUseCase(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<ChannelDto?> ExecuteAsync(int channelId)
    {
        var channel = await _channelRepository.GetByIdAsync(channelId);
        if (channel is null)
            return null;

        return channel.ToChannelDto();
    }
}