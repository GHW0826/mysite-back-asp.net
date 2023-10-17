using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace mysite_back_asp.net
{
    public static class Extension
    {

        public static IApplicationBuilder UseExtension(this IApplicationBuilder app, string healthcheck = "/api/healthcheck")
        {
            var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                }
            );

            if (!environment.IsProduction())
            {

            }

            return app;
        }
    }
}
