using System.ComponentModel.DataAnnotations;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class CreateUserDto
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

    public User ToUser()
    {
        return new User
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Username = Username,
            Email = Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password)
        };
    }
}
