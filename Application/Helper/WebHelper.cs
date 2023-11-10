using Application.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Application.Helper;

public class WebHelper
{
    private const string REFRESH_TOKEN_COOKIE_NAME = "RTC";
    private readonly IHttpContextAccessor _context;
    private readonly AppConfig _appConfig;
    private readonly CookieOptions _refreshTokenCookieOption;
    private readonly CookieOptions _removeCookieOption;

    public WebHelper(IHttpContextAccessor context, AppConfig appConfig)
    {
        _context = context;
        _appConfig = appConfig;

        if (_appConfig.Token == null)
            throw new ArgumentException("");

        _refreshTokenCookieOption = new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Expires = DateTime.Now.AddMinutes(_appConfig.Token.RefreshCookieExpired),
            Domain = _appConfig.CookieDomain,
            Path = "/api/v1/auth/refresh"
        };

        _removeCookieOption = new CookieOptions()
        {
            Secure = true,
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            Domain = _appConfig.CookieDomain,
            Path = "/api/v1/auth/refresh"
        };
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await GetTokenValueAsync("access_token");
    }

    private async Task<string?> GetTokenValueAsync(string key)
    {
        return await _context.HttpContext?.GetTokenAsync(key);
    }

    public string GetAccessTokenWithBearer()
    {
        return _context.HttpContext?.Request.Headers["Authorization"].ToString() ?? "";
    }

    public string GetAuthorizationHeaderToken()
    {
        return GetAccessTokenWithBearer().Replace("Bearer ", "").Trim();
    }

    public string GetRefreshToken()
    {
        return _context.HttpContext?.Request.Cookies[REFRESH_TOKEN_COOKIE_NAME] ?? "";
    }

    public void SetRefreshToken(string refreshToken)
    {
        _context.HttpContext?.Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, refreshToken, _refreshTokenCookieOption);
    }

    public void RemoveRefreshToken()
    {
        RemoveCookie(REFRESH_TOKEN_COOKIE_NAME);
    }

    public void RemoveCookie(string name)
    {
        _context?.HttpContext?.Response.Cookies.Delete(name, _removeCookieOption);
    }
}
