using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> ExecuteAsync(UserCreateDto request)
    {
        var user = new User
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            CreatedOn = user.CreatedOn,
            ModifiedOn = user.ModifiedOn
        };
    }
}