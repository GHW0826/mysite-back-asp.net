namespace Application.User.Model;

public class SignUpRequestModel
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
}
