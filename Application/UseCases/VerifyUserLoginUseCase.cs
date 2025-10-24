using System.Security.Authentication;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class VerifyUserLoginUseCase
{
    private readonly IUserRepository _userRepository;

    public VerifyUserLoginUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> ExecuteAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new InvalidCredentialException("Invalid credential. Please try again");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new InvalidCredentialException("Invalid credential. Please try again");

        return user.ToUserDto();
    }
}