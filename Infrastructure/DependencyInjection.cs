
using Application.Auth;
using Application.Auth.Interface;
using Application.Interface;
using Application.Interface.Sharding;
using Application.Test;
using Infrastructure.adapter;
using Infrastructure.Auth;
using Infrastructure.Helper;
using Infrastructure.Repository;
using Infrastructure.Shard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        #region Database

        // Mysql
        services.AddDbContextPool<MysqlContext>(
            o => o.UseMySQL(configuration.GetConnectionString("MysqlContext:Shard0") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
        );


        /*
        services.AddDbContextPool<AuthContext>(
    o => o.UseMySQL(configuration.GetConnectionString("MysqlContext:Shard0") ?? throw new ArgumentNullException("AuthContext Argument is null"))
);
        services.AddScoped<IAuthDbContext, AuthContext>();
        */

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "127.0.0.1:6379, defaultDatabase=0";
            options.InstanceName = string.Empty;// "master";
        });

        //services.AddDistributedMemoryCache();

        #endregion


        #region ContextPool

        // shard manager info
        services.BeginRegisteringDbContextPoolMultiplexerService<ShardInfoContext>()
            .AddConnectionDetails("auth:shardMange:0", 
                (provider, builder) => builder.UseMySQL(
                    configuration.GetConnectionString("MysqlContext:Shard0") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
                )
            .AddConnectionDetails("auth:shardMange:1",
                (provider, builder) => builder.UseMySQL(
                    configuration.GetConnectionString("MysqlContext:Shard1") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
                )
        .FinishRegisteringDbContextPoolMultiplexerService();

        // auth
        services.BeginRegisteringDbContextPoolMultiplexerService<AuthContext>()
            .AddConnectionDetails("auth:shardMange:0",
                (provider, builder) => builder.UseMySQL(
                    configuration.GetConnectionString("MysqlContext:Shard0") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
                )
            .AddConnectionDetails("auth:shardMange:1",
                (provider, builder) => builder.UseMySQL(
                    configuration.GetConnectionString("MysqlContext:Shard1") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
                )
        .FinishRegisteringDbContextPoolMultiplexerService();



        services.AddSingleton<IShardContextPoolInterface, ShardContextPoolWarpper>();
        services.AddSingleton<IAuthContextPoolInterface, AuthContextPoolWarpper>();

        #endregion

        services.AddScoped<IDatabaseAsync>(cfg =>
        {
            IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379, defaultDatabase=0");
            return multiplexer.GetDatabase();
        });
        services.AddTransient<IDistributedLockContext, DistributedLockContext>();
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

    /// <summary>
    /// Extension method to bootstrap creation of <see cref="DbContextPoolMultiplexerServiceBuilder{T}" />.
    /// </summary>
    /// <typeparam name="T">A type that extends DbContext.</typeparam>
    /// <param name="services">An instance of <see cref="IServiceCollection" />.</param>
    /// <returns>An instance of <see cref="DbContextPoolMultiplexerServiceBuilder{T}" />.</returns>
    public static DbContextPoolMultiplexerServiceBuilder<T> BeginRegisteringDbContextPoolMultiplexerService<T>(
        this IServiceCollection services
        ) where T : DbContext
    {
        return new DbContextPoolMultiplexerServiceBuilder<T>(services);
    }
}
