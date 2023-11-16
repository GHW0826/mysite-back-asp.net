
using Microsoft.AspNetCore.Mvc;
using SnowFall.AspNetCore;

namespace mysite_back_asp.net.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _config;

    public UserController(ILogger<AuthController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
}
