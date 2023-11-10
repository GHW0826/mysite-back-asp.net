
namespace Application.Auth.Model;

public class AuthorizeResponseModel
{
    public long id { get; set; }
    public string accessToken { get; set; } = string.Empty;
    public string refreshToken { get; set; } = string.Empty;
}
