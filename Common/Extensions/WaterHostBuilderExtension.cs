using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Water.Common.AspNetCore.Extensions;

public static class WaterHostBuilderExtension
{
    // 요약:
    //     API 구성에 필요한 Marco 기본 구성들을 Service Collection에 추가합니다.
    // 매개 변수:
    //   services:
    //   configuration:
    //   hostBuilder:
    // 예외:
    //   T:System.ArgumentNullException:
    public static IHostBuilder AddWaterHostBuilderExtension<T>(this IHostBuilder hostBuilder, Action<ConfigOptions, T> configAction) where T : BaseConfig
    {
        // T : AppConfig
        var configOptions = new ConfigOptions();

        // config setting
        hostBuilder.AddWaterConfig<T>(configOptions);
        
        // Use Serilog
        hostBuilder.AddWaterSerilog(configOptions);


        // 호스트 빌더를 설정하고, 서비스 컨테이너에 서비스를 추가하는 부분.
        hostBuilder.ConfigureServices((context, services) =>
        {
            //서비스 컨테이너에서 타입 T의 서비스 인스턴스를 가져옴.
            // 서비스는 T로 지정된 Configuration 클래스
            var config = services.BuildServiceProvider().GetService<T>()
                ?? throw new Exception("GetService<T> excuted T is invalid in AddWaterHostBuilderExtension");

            // 지정된 configOptions와 인자로 받은 configAction 델리게이트를 호출.
            // 설정된 구성 값을 사용해 애플리케이션을 구성하고 설정.
            configAction.Invoke(configOptions, config);
        });

        return hostBuilder;
    }

    /// <summary>
    /// Marco 기본 설정으로 Serilog를 설정합니다.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="hostBuilder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static IHostBuilder AddWaterSerilog(this IHostBuilder hostBuilder, ConfigOptions configOptions)
    {
        hostBuilder.ConfigureAppConfiguration((context, configuation) =>
        {
            configuation.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
        });

        hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.ApplicationInsights(services.GetService<TelemetryConfiguration>(), TelemetryConverter.Events);

            configOptions.DestructureAction?.Invoke(loggerConfiguration.Destructure);
        });

        return hostBuilder;
    }

    private static IHostBuilder AddWaterConfig<T>(this IHostBuilder hostBuilder, ConfigOptions configOptions) where T : BaseConfig
    {
        hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            T appconfig = context.Configuration.GetSection("AppConfig").Get<T>() 
                ?? throw new ArgumentException("Config Argument is Null Check \"AppConfig\" argument in appsettings.json");

            hostBuilder.ConfigureServices((context, services) =>
            {
                foreach (var item in context.Configuration.GetChildren())
                {
                    appconfig[item.Key] = item.Value ?? "";
                }

                services.AddSingleton(appconfig);
                services.AddSingleton<BaseConfig>(appconfig);
                services.AddSingleton(configOptions);
            });
        });

        return hostBuilder;
    }
}
