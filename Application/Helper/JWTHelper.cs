using Application.Auth.Model;
using Application.Interface;
using Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Helper;

public class JWTHelper : ITokenHelper
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _securityKey;
    private readonly SigningCredentials _credentials;
    private static readonly double _accessTokenExpireInMinutes;
    private static readonly double _refreshTokenExpireInDays;

    public JWTHelper(IConfiguration config)
    {
        _config = config;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt Secretkey is null in appsettings")));
        _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512);
    }

    public static DateTime GetDefaultAccessTokenExpireTime()
    {
        return DateTime.Now.AddMinutes(_accessTokenExpireInMinutes);
    }

    public static DateTime GetDefaultRefreshTokenExpireTime()
    {
        return DateTime.Now.AddDays(_refreshTokenExpireInDays);
    }

    public string GenerateJWTToken(TokenInfo info, DateTime expireTime)
    {
        var claims = MakeClaims(info);
        var token = new JwtSecurityToken (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddSeconds(20),
            signingCredentials: _credentials
        );

        // JwtSecurityTokenHandler is not thread safe
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Dictionary<string, string> ExtractClaimsFromJwt(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken
                            ?? throw new ArgumentException("Invalid JWT token");

        Dictionary<string, string> claimsDictionary = new Dictionary<string, string>();
        foreach (var claim in jsonToken.Claims)
        {
            claimsDictionary.Add(claim.Type, claim.Value);
        }
        return claimsDictionary;
    }

    public string GenerateAccessToken(TokenInfo info)
    {
        return GenerateJWTToken(info, GetDefaultAccessTokenExpireTime());
    }

    public string GenerateRefreshToken(TokenInfo info)
    {
        return GenerateJWTToken(info, GetDefaultRefreshTokenExpireTime());
    }

    public Claim[] MakeClaims(TokenInfo info)
    {
        var claims = new[]
{
            new Claim(JwtRegisteredClaimNames.Sub, info.name),
            new Claim(JwtRegisteredClaimNames.Name, info.name),
            new Claim("id", info.id.ToString()),
            new Claim("email", info.email),
            new Claim("userRole", info.userRole),
            new Claim(ClaimTypes.Name, info.name),
            new Claim(ClaimTypes.Role, info.userRole),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        return claims;
    }


    /*
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var Key = Encoding.UTF8.GetBytes(_config["JWT:Key"]);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    */
}
