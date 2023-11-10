
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Application.Test;
using Infrastructure.adapter;
using Infrastructure.Helper;
using Infrastructure.Repository;
using Infrastructure.Shard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        #region Database

        /*
        // Mysql
        services.AddDbContextPool<MysqlContext>(
            o => o.UseMySQL(configuration.GetConnectionString("MysqlContext:Shard0") ?? throw new ArgumentNullException("MysqlContext Argument is null"))
        );
        */

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

        // Shard Info context
        var connectionStrings = configuration?.GetSection("ConnectionStrings");
        if (connectionStrings != null)
        {
            // shard manager info
            var shardInfoShardCnt = connectionStrings.GetChildren()
                        .Where(b => b.Key.StartsWith(ShardKeyHelper.SHARD_INFO_APPSETTING_KEY))
                        .SelectMany(subSection => subSection.GetChildren())
                        .ToList().Count();
            var shardInfoContextPoolMS = services.BeginRegisteringDbContextPoolMultiplexerService<ShardInfoContext>();
            for (int i = 0; i < shardInfoShardCnt; ++i)
            {
                var contextName = ShardKeyHelper.SHARD_INFO_CONTEXT_KEY + i.ToString();
                var connectionString = configuration?.GetConnectionString(ShardKeyHelper.PREFIX_SHARD_INFO_CONNECTION_STRING + i.ToString())
                            ?? throw new ArgumentNullException("ShardInfo Context Argument is null");
                shardInfoContextPoolMS.AddConnectionDetails(
                        contextName,
                        (provider, builder) => builder.UseMySQL(connectionString));
            }
            shardInfoContextPoolMS.FinishRegisteringDbContextPoolMultiplexerService();

            ///////////////////////////////////////////////////////////////////////
            
            // user context
            var userShardCnt = connectionStrings.GetChildren()
                        .Where(b => b.Key.StartsWith(ShardKeyHelper.USER_APPSETTING_KEY))
                        .SelectMany(subSection => subSection.GetChildren())
                        .ToList().Count();
            var userContextPoolMS = services.BeginRegisteringDbContextPoolMultiplexerService<UserContext>();
            for (int i = 0; i < userShardCnt; ++i)
            {
                var contextName = ShardKeyHelper.USER_CONTEXT_KEY + i.ToString();
                var connectionString = configuration?.GetConnectionString(ShardKeyHelper.PREFIX_USER_CONNECTION_STRING + i.ToString())
                            ?? throw new ArgumentNullException("User Context Argument is null");
                userContextPoolMS.AddConnectionDetails(
                    contextName,
                    (provider, builder) => builder.UseMySQL(connectionString));
            }
            userContextPoolMS.FinishRegisteringDbContextPoolMultiplexerService();

            // auth token context
            var authTokenCnt = connectionStrings.GetChildren()
                                .Where(b => b.Key.StartsWith(ShardKeyHelper.AUTH_TOKEN_APPSETTING_KEY))
                                .SelectMany(subSection => subSection.GetChildren())
                                .ToList().Count();
            var authTokenContextPoolMS = services.BeginRegisteringDbContextPoolMultiplexerService<AuthTokenContext>();
            for (int i = 0; i < authTokenCnt; ++i)
            {
                var contextName = ShardKeyHelper.AUTH_TOKEN_CONTEXT_KEY + i.ToString();
                var connectionString = configuration?.GetConnectionString(ShardKeyHelper.PREFIX_AUTH_TOKEN_CONNECTION_STRING + i.ToString())
                            ?? throw new ArgumentNullException("AuthTokenContext Argument is null");
                authTokenContextPoolMS.AddConnectionDetails(
                    contextName,
                    (provider, builder) => builder.UseMySQL(connectionString));
            }
            authTokenContextPoolMS.FinishRegisteringDbContextPoolMultiplexerService();

            services.AddSingleton<IShardInfoContextPool,    ShardInfoContextPoolWarpper>();
            services.AddSingleton<IUserContextPool,         UserContextPoolWarpper>();
            services.AddSingleton<IAuthTokenContextPool,    AuthTokenContextPoolWarpper>();
        }

        #endregion

        services.AddScoped<IDatabaseAsync>(cfg =>
        {
            IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379, defaultDatabase=0");
            return multiplexer.GetDatabase();
        });
        services.AddTransient<IDistributedLockContext, DistributedLockContext>();
        services.AddScoped<ITestAdapter, TestAdapter>();



        return services;
    }

    /// <summary>
    /// Extension method to bootstrap creation of <see cref="DbContextPoolMultiplexerServiceBuilder{T}" />.
    /// </summary>
    /// <typeparam name="T">A type that extends DbContext.</typeparam>
    /// <param name="services">An instance of <see cref="IServiceCollection" />.</param>
    /// <returns>An instance of <see cref="DbContextPoolMultiplexerServiceBuilder{T}" />.</returns>
    public static DbContextPoolMultiplexerServiceBuilder<T> BeginRegisteringDbContextPoolMultiplexerService<T>
        (this IServiceCollection services) where T : DbContext
    {
        return new DbContextPoolMultiplexerServiceBuilder<T>(services);
    }
}
