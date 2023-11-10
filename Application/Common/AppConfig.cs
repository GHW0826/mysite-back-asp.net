using Water.Common;

namespace Application.Common;

public class AppConfig : BaseConfig
{
    public string CookieDomain { get; set; } = string.Empty;

    public TokenConfig? Token { get; set; }

    public class TokenConfig
    {
        public int AccessExpired { get; set; }
        public int RefreshExpired { get; set; }
        public int RefreshCookieExpired { get; set; }
    }

}
