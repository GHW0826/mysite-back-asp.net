using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string commandName = request.ToString() ?? "";

        TResponse response = await next();

        _logger.LogDebug("Response {commandName} {@request} {@response}", commandName, request, response);
        
        return response;
    }
}