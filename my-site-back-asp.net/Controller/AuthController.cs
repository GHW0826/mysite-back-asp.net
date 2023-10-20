using Application.Test.queries;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Water.Common.AspNetCore;

namespace mysite_back_asp.net.Controller;



[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sign Up Users
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /*
    [HttpGet("{id}")]
    public async Task<ActionResult<TestEntity>> GetUserInfo(string email)
    {
        var users = await _context.TestEntitys.FindAsync(email);

        if (users == null)
        {
            return NotFound();
        }

        return users;
    }
    */

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<TestEntity>> GetUserInfo(string email)
    {
        return new ActionResult<TestEntity>(Ok());
    }

    [Authorize]
    [HttpGet("authenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        TestQuery query2 = new();
        // GetAuthenticatedQuery query = new();
        var IsAuthenticated = await Mediator.Send(query2);

        return Ok(new { Code = "0", IsAuthenticated = IsAuthenticated });
    }
}
