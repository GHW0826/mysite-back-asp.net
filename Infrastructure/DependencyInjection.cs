
using Application.Auth;
using Application.Auth.Interface;
using Application.Interface;
using Application.Test;
using Infrastructure.adapter;
using Infrastructure.Auth;
using Infrastructure.Helper;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Water.Common;
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

        // AuthContext
        services.AddDbContextPool<AuthContext>(
            o => o.UseMySQL(configuration.GetConnectionString("MysqlContext") ?? throw new ArgumentNullException("AuthContext Argument is null"))
        );

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "127.0.0.1:6379, defaultDatabase=0";
            options.InstanceName = string.Empty;// "master";
        });

        //services.AddDistributedMemoryCache();

        #endregion
        services.AddScoped<IDatabaseAsync>(cfg =>
        {
            IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379, defaultDatabase=0");
            return multiplexer.GetDatabase();
        });
        services.AddTransient<IDistributedLockContext, DistributedLockContext>();
        services.AddScoped<IAuthDbContext, AuthContext>();
        services.AddScoped<ITestAdapter, TestAdapter>();


        #region Auth
        // JwtBearerOptions t;

        services.AddSingleton<ITokenHelper, JWTHelper>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
        //    options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt Secretkey is null in appsetting"))),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(config =>
        {
            config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
            config.AddPolicy(Policies.User, Policies.UserPolicy());
            config.AddPolicy(Policies.UserName, Policies.UserNamePolicy());
        });

        #endregion


        return services;
    }
}
