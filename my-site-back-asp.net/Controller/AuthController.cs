using Application.Auth.Command;
using Application.Auth.Model;
using Application.Auth.Query;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Water.Common.AspNetCore;

namespace mysite_back_asp.net.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;
    private readonly WebHelper _webHelper;

    public AuthController(ILogger<AuthController> logger, WebHelper webHelper)
    {
        _logger = logger;
        _webHelper = webHelper;
    }

    /// <summary>
    /// 인증, 인증 성공시 토큰 로직 동작.
    /// </summary>
    /// <param name="requset"></param>
    /// <returns></returns>
    [HttpPost("authorize")]
    public async Task<IActionResult> Authorize(AuthorizeRequestModel requset)
    {
        var result = await Mediator.Send(new AuthorizeCommand()
        {
            email = requset.email,
            password = requset.password
        });

        // set refresh token in cookie
        _webHelper.SetRefreshToken(result.refreshToken);

        return success(result);
    }

    /// <summary>
    /// 토큰 갱신
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh/{id}")]
    public async Task<IActionResult> RefreshToken(long id)
    {
        var token = await _webHelper.GetAccessTokenAsync();
        // 만료되지 않은 상태에서 refresh 호출시 그대로 액세스 토큰 리턴
        if (string.IsNullOrEmpty(token) == false)
            return Ok(new { accessToken = token });

        var requestAccessToken = _webHelper.GetAuthorizationHeaderToken();
        var requestRefreshToken = _webHelper.GetRefreshToken();
        var result = await Mediator.Send(new RefreshQuery()
        {
            id = id,
            accessToken = requestAccessToken,
            refreshToken = requestRefreshToken
        });
        // set refresh token in cookie
        _webHelper.SetRefreshToken(requestRefreshToken);

        return success(result);
    }

    /// <summary>
    /// 로그아웃
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _webHelper.RemoveRefreshToken();
        return Ok();
    }

}
