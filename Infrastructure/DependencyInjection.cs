
using Application.Test;
using Infrastructure.adapter;
using Infrastructure.Auth;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Water.Common.Interfaces;

namespace Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        #region Database

        /*
        services.AddDbContext<MysqlContext>((options) =>
        {
            options.UseMySQL(configuration.GetConnectionString("MysqlContext") ?? "");
        });
        */


        // Mysql
        services.AddDbContextPool<MysqlContext>(
            o => o.UseMySQL(configuration.GetConnectionString("MysqlContext") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
        );

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "127.0.0.1:6379, defaultDatabase=0";
            options.InstanceName = string.Empty;// "master";
        });


        #endregion

        services.AddScoped<ITestAdapter, TestAdapter>();


        #region Auth

        services.AddScoped<IAuthUser, AuthUser>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies["EpochAccessToken"];
                    if (!string.IsNullOrEmpty(accessToken))
                        context.Token = accessToken;

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    return Task.CompletedTask;
                },
            };
        });

     //   services.ConfigureOptions<EpochJwtBearerOptions>();
        
        #endregion


        return services;
    }
}
