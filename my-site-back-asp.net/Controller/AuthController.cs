using Application.Auth.Command;
using Application.Auth.Interface;
using Application.Auth.Query;
using Application.Test.queries;
using Infrastructure.Auth;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Water.Common.AspNetCore;

namespace mysite_back_asp.net.Controller;



[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _config;
    private readonly ITokenHelper _tokenHelper;


    public AuthController(ILogger<AuthController> logger, IConfiguration config, ITokenHelper tokenHelper)
    {
        _logger = logger;
        _config = config;
        _tokenHelper = tokenHelper;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> signUp([FromBody] AuthUser login)
    {
        var result = await Mediator.Send(new SignUpCommand()
        {
            email = login.Email,
            password = login.Password,
            name = login.Name
        });
        return Ok(new
        {
            userDetails = result
        });
    }

    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> signin([FromBody] AuthUser login)
    {
        var result = await Mediator.Send(new SignInQuery()
        {
            email = login.Email,
            password = login.Password
        });

        IActionResult response = Unauthorized();
        if (result != null)
        {
            var jwtToken = _tokenHelper.GenerateJWTToken(login);
            response = Ok(new
            {
                token = jwtToken,
                user = result,
            });
        }
        return response;
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

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] AuthUser login)
    {
        IActionResult response = Unauthorized();
     //   var tokenString = GenerateJWTToken(login);
     //   response = Ok(new
     //   {
    //        token = tokenString,
     //       userDetails = login,
     //   });
        /*
        AuthUser user = AuthenticateUser(login);
        if (user != null)
        {
            var tokenString = GenerateJWTToken(login);
            response = Ok(new
            {
                token = tokenString,
                userDetails = login,
            });
        }
        */
        return response;
    }



    [HttpPost("test_auth")]
    [AllowAnonymous]
    public async Task<IActionResult> TestLogin([FromBody] AuthUser login)
    {
        var result = await Mediator.Send(new SignInQuery()
        {
            email = login.Email,
            password = login.Password
        });

        IActionResult response = Unauthorized();
        /*
        if (result != null)
        {
            var tokenString = GenerateJWTToken(login);
            response = Ok(new
            {
                token = tokenString,
                userDetails = login,
            });
        }
        */
        return response;
    }


    [Authorize(Policy = Policies.UserName)]
    [HttpGet("authenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        TestQuery query2 = new();
        // GetAuthenticatedQuery query = new();
        var IsAuthenticated = await Mediator.Send(query2);

        return Ok(new { Code = "0", IsAuthenticated = IsAuthenticated });
    }
}
