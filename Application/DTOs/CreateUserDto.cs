using System.ComponentModel.DataAnnotations;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class CreateUserDto
{
    [MaxLength(100)]
    public required string Firstname { get; set; }
    [MaxLength(100)]
    public required string Lastname { get; set; } = string.Empty;

    [MaxLength(50)]
    public required string Username { get; set; }

    [MaxLength(256)]
    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(6)]
    [MaxLength(20)]
    public required string Password { get; set; }

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
