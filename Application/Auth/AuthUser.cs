

namespace Infrastructure.Auth;

public class AuthUser
{
    /*
    public bool IsAuthenticated => throw new NotImplementedException();

    public string Pid => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public string UserIp => throw new NotImplementedException();

    public string Roles => throw new NotImplementedException();
    */
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
}
