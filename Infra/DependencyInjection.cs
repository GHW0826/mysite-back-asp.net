
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Application.Test;
using Infra.adapter;
using Infra.Repository;
using Infra.Shard;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infra;

public static class DependencyInjection
{

    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {

        #region Database

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
        if (connectionStrings != null && configuration != null)
        {
            // shard manager info
            var shardInfoShardCnt = connectionStrings.GetChildren()
                        .Where(b => b.Key.StartsWith(ShardKeyHelper.SHARD_INFO_APPSETTING_KEY))
                        .SelectMany(subSection => subSection.GetChildren())
                        .ToList().Count();
            var shardInfoContextName = ShardKeyHelper.SHARD_INFO_CONTEXT_KEY;
            var shardInfoConnectionStringKey = ShardKeyHelper.PREFIX_SHARD_INFO_CONNECTION_STRING;
            services.AddSnowFallContextPool<ShardInfoContext>(configuration, shardInfoContextName, shardInfoConnectionStringKey, shardInfoShardCnt);

            // user context
            var userShardCnt = connectionStrings.GetChildren()
                        .Where(b => b.Key.StartsWith(ShardKeyHelper.USER_APPSETTING_KEY))
                        .SelectMany(subSection => subSection.GetChildren())
                        .ToList().Count();
            var userContextName = ShardKeyHelper.USER_CONTEXT_KEY;
            var userConnectionStringKey = ShardKeyHelper.PREFIX_USER_CONNECTION_STRING;
            services.AddSnowFallContextPool<UserContext>(configuration, userContextName, userConnectionStringKey, userShardCnt);

            // auth token context
            var authTokenCnt = connectionStrings.GetChildren()
                                .Where(b => b.Key.StartsWith(ShardKeyHelper.AUTH_TOKEN_APPSETTING_KEY))
                                .SelectMany(subSection => subSection.GetChildren())
                                .ToList().Count();
            var authContextName = ShardKeyHelper.AUTH_TOKEN_CONTEXT_KEY;
            var authConnectionStringKey = ShardKeyHelper.PREFIX_AUTH_TOKEN_CONNECTION_STRING;
            services.AddSnowFallContextPool<AuthTokenContext>(configuration, authContextName, authConnectionStringKey, authTokenCnt);


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
}
