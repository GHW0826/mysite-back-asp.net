
using Microsoft.AspNetCore.Http;

namespace Application.Auth.Model;

public class TokenInfo
{
    public long id { get; set; }
    public string email { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;

    public static TokenInfo Gen(long id, string email, string name, string userRole)
    {
        return new TokenInfo
        {
            id = id,
            email = email,
            name = name,
            userRole = userRole
        };
    }
}
