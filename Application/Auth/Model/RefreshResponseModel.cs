
namespace Application.Auth.Model;

public class RefreshResponseModel
{
    public long id { get; set; }
    public string accessToken { get; set; } = string.Empty;
}
