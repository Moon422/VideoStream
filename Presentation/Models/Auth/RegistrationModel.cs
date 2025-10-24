using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Presentation.Models.Auth;

public record RegistrationModel : BaseModel
{
    [Required, MaxLength(128)]
    public string Firstname { get; set; }

    [Required, MaxLength(128)]
    public string Lastname { get; set; }

    [Required, MaxLength(128)]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, PasswordPropertyText]
    public string Password { get; set; }
}