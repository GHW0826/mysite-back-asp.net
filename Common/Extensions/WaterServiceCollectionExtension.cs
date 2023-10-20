
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Water.Common.AspNetCore.Extensions;

public static class WaterServiceCollectionExtension
{
    public static IServiceCollection AddWaterServiceCollectionExtenstion(this IServiceCollection services)
    {
        // Use MediatR
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddEndpointsApiExplorer();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

/*
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "my-site",
                ValidAudience = "my-site",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("holywater"))
            };
        });
*/

        return services;
    }
}
