using Application.Auth.Model;
using Infrastructure.Auth;
using System.Security.Claims;

namespace Application.Interface;

public interface ITokenHelper
{
    public Dictionary<string, string> ExtractClaimsFromJwt(string jwtToken);
    public string GenerateJWTToken(TokenInfo info, DateTime expireTime);
    public string GenerateAccessToken(TokenInfo info);
    public string GenerateRefreshToken(TokenInfo info);
}
