using System.ComponentModel.DataAnnotations;

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
}
