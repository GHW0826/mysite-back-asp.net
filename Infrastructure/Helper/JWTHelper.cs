using Application.Auth.Interface;
using Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Water.Common.Interfaces;

namespace Infrastructure.Helper;

public class JWTHelper : ITokenHelper
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _securityKey;
    private readonly SigningCredentials _credentials;

    public JWTHelper(IConfiguration config)
    {
        _config = config;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt Secretkey is null in appsettings")));
        _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512);
    }

    public string GenerateJWTToken(AuthUser userInfo)
    {
        var claims = new[]
{
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
            new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
            new Claim(ClaimTypes.Name, userInfo.UserName),
            new Claim(ClaimTypes.Role, userInfo.UserRole),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddSeconds(20),
            signingCredentials: _credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
