
using Water.Common.Interfaces;

namespace Infrastructure.Auth;

public class AuthUser : IAuthUser
{
    public bool IsAuthenticated => throw new NotImplementedException();

    public string Pid => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public string UserIp => throw new NotImplementedException();

    public string Roles => throw new NotImplementedException();
}
