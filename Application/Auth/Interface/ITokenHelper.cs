using Infrastructure.Auth;

namespace Application.Auth.Interface;

public interface ITokenHelper
{
    public string GenerateJWTToken(AuthUser userInfo);
}
