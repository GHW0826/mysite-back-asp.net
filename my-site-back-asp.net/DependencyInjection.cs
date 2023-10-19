
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mysite_back_asp.net.Repository;

namespace mysite_back_asp.net
{
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


            return services;
        }
    }
}
