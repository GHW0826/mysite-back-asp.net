using Serilog.Configuration;
using Water.Common.AspNetCore.Filters;

namespace Water.Common;

public class ConfigOptions
{
    internal Type ExceptionFilter { get; set; }

    public void AddExceptionFilter<T>() where T : BaseExceptionFilterAttribute
    {
        ExceptionFilter = typeof(T);
    }

    internal Action<LoggerDestructuringConfiguration>? DestructureAction { get; set; }
}
