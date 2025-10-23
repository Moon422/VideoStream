using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class GetUserByUsernameUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> ExecuteAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user is null)
            return null;

        return user.ToUserDto();
    }
}