using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace mysite_back_asp.net.Controller;

[ApiController]
public class ErrorHandler : ControllerBase
{
    private readonly ILogger<ErrorHandler> _logger;
    private readonly IConfiguration _config;


    public ErrorHandler(ILogger<ErrorHandler> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult error([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return Problem(title: exceptionHandlerFeature.Error.Message);
    }

}
