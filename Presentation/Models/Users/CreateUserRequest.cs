using System.ComponentModel.DataAnnotations;
using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Users;

public record CreateUserRequest : BaseModel
{
    [MaxLength(100)]
    public required string Firstname { get; set; }

    [MaxLength(100)]
    public required string Lastname { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }

    [MaxLength(256)]
    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(6)]
    [MaxLength(20)]
    public required string Password { get; set; }

    public CreateUserDto ToCreateUserDto()
    {
        return new CreateUserDto
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Username = Username,
            Email = Email,
            Password = Password
        };
    }
}
