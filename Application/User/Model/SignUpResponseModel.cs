namespace Application.User.Model;

public class SignUpResponseModel
{
    public long id { get; set; }

    public string email { get; set; } = string.Empty;

    public string name { get; set; } = string.Empty;
}
