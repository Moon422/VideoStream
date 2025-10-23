using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class GetUserByEmailUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> ExecuteAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null)
            return null;

        return user.ToUserDto();
    }
}
