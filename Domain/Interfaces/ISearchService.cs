using System.Threading.Tasks;
using VideoStream.Domain.Entities;

namespace VideoStream.Domain.Interfaces;

public interface ISearchService
{
    Task IndexVideoAsync(Video video);
    Task IndexChannelAsync(Channel channel);
    Task<IPagedList<BaseEntity>> SearchAsync(string query, int page, int pageSize);
    Task RemoveVideoAsync(int videoId);
    Task RemoveChannelAsync(int channelId);
}