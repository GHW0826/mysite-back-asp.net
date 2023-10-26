using Application.Test;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Auth.Model;

namespace Application.Auth.Query;

/// <summary>
/// Sign In 
/// </summary>
public class SignInQuery : IRequest<SignInModel>
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}

/// <summary>
/// SignInQuery 를 처리합니다.
/// </summary>
public class SignInQueryHandler : IRequestHandler<SignInQuery, SignInModel?>
{
    private readonly ILogger<SignInQueryHandler> _logger;
    private readonly IAuthDbContext _context;

    public SignInQueryHandler(ILogger<SignInQueryHandler> logger, IAuthDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<SignInModel?> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _context.findByEmailAndPassword(request.email, request.password) ?? null;
        return new SignInModel {
            email = request.email
        };
    }
}

