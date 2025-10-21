using System;
using VideoStream.Application.DTOs;

namespace VideoStream.Presentation.Models.Users;

public record UserModel : BaseEntityModel
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public static class UserDtoToUserModelHelper
{
    public static UserModel ToUserModel(this UserDto userDto)
    {
        return new UserModel
        {
            Id = userDto.Id,
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Username = userDto.Username,
            Email = userDto.Email,
            IsAdmin = userDto.IsAdmin,
            CreatedOn = userDto.CreatedOn,
            ModifiedOn = userDto.ModifiedOn
        };
    }
}