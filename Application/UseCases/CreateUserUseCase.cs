using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> ExecuteAsync(CreateUserDto request)
    {
        var user = await _userRepository.AddAsync(request.ToUser());
        return user.ToUserDto();
    }
}