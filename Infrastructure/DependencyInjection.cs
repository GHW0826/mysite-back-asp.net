
using Application.Test;
using Infrastructure.adapter;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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


        return services;
    }
}
