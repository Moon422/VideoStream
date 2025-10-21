using System;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public static class UserToUserDtoHelper
{
    public static UserDto ToUserDto(this User user)
    {
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
