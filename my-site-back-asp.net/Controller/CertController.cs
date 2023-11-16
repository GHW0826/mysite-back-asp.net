using Application.User.Command;
using Application.User.Model;
using Microsoft.AspNetCore.Mvc;
using SnowFall.AspNetCore;

namespace mysite_back_asp.net.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class CertController : BaseApiController
{
    private readonly ILogger<CertController> _logger;

    public CertController(ILogger<CertController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sign Up
    /// </summary>
    /// <param name="requset"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<IActionResult> signUp(SignUpRequestModel requset)
    {
        var result = await Mediator.Send(new SignUpCommand()
        {
            email = requset.email,
            password = requset.password,
            name = requset.name,
            userRole = requset.userRole,
        });

        return created("/api/[controller]/signup", result);
    }
}
