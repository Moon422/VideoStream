using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using VideoStream.Domain.Pagination;

namespace VideoStream.Infrastructure.Services;

public class AlgoliaSearchService : ISearchService
{
    private readonly ILogger<AlgoliaSearchService> _logger;
    private readonly IPagedListFactory _pagedListFactory;

    public AlgoliaSearchService(ILogger<AlgoliaSearchService> logger, IPagedListFactory pagedListFactory)
    {
        _logger = logger;
        _pagedListFactory = pagedListFactory;
    }

    public Task IndexVideoAsync(Video video)
    {
        _logger.LogInformation("Indexing video {VideoId}: {Title}", video.Id, video.Title);
        return Task.CompletedTask;
    }

    public Task IndexChannelAsync(Channel channel)
    {
        _logger.LogInformation("Indexing channel {ChannelId}: {Name}", channel.Id, channel.Name);
        return Task.CompletedTask;
    }

    public Task RemoveVideoAsync(int videoId)
    {
        _logger.LogInformation("Removing video {VideoId} from index", videoId);
        return Task.CompletedTask;
    }

    public Task RemoveChannelAsync(int channelId)
    {
        _logger.LogInformation("Removing channel {ChannelId} from index", channelId);
        return Task.CompletedTask;
    }

    public Task<IPagedList<BaseEntity>> SearchAsync(string query, int page, int pageSize)
    {
        _logger.LogInformation("Search requested: {Query}", query);
        // Placeholder: return empty results
        return Task.FromResult(_pagedListFactory.Create(new List<BaseEntity>(), 0, page, pageSize));
    }
}
