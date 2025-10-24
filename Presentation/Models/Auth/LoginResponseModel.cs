using VideoStream.Presentation.Models.Users;

namespace VideoStream.Presentation.Models.Auth;

public record LoginResponseModel : BaseModel
{
    public UserModel User { get; set; }
    public string Jwt { get; set; }
}