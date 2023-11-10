
namespace Application.Auth.Model;

public class AuthorizeRequestModel
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}