using System.Threading.Tasks;
using VideoStream.Application.DTOs;

namespace VideoStream.Application.Interfaces;

public interface IWorkContext
{
    Task<UserDto?> GetCurrentUserAsync();
}