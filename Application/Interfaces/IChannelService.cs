using System.Threading.Tasks;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.Interfaces;

public interface IChannelService
{
    Task<Channel?> GetByIdAsync(int id);
    Task<int> CreateAsync(string name, string description, int createdByUserId);
}
