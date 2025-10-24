namespace VideoStream.Presentation.Models.Auth;

public record LoginModel : BaseModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
