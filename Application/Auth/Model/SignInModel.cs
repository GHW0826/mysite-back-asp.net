
namespace Application.Auth.Model;

public class SignInModel
{
    public long id { get; set; }
    public string email { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}
