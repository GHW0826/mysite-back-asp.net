using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace mysite_back_asp.net
{
    public static class Extension
    {

        public static IApplicationBuilder UseExtension(this IApplicationBuilder app, string healthcheck = "/api/healthcheck")
        {
            return app;
        }
    }
}
