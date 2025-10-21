using System.ComponentModel.DataAnnotations;
using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Users;

public record CreateUserRequest : BaseModel
{
    [MaxLength(100)]
    public string Firstname { get; set; }

    [MaxLength(100)]
    public string Lastname { get; set; }

    [MaxLength(50)]
    public string Username { get; set; }

    [MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; }

    [MinLength(6)]
    [MaxLength(20)]
    public string Password { get; set; }

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
