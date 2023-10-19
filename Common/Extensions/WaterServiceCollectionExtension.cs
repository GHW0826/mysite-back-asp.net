
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Water.Common.AspNetCore.Extensions;

public static class WaterServiceCollectionExtension
{
    public static IServiceCollection AddWaterServiceCollectionExtenstion(this IServiceCollection services)
    {
        // Use MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddEndpointsApiExplorer();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        return services;
    }
}
