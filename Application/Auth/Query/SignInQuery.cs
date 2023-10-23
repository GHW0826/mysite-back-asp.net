using Application.Test;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Auth.Model;

namespace Application.Auth.Query;

public class SignInQuery : IRequest<SignInModel>
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}

public class SignInQueryHandler : IRequestHandler<SignInQuery, SignInModel>
{
    //  readonly ITestDbContext _context;
    readonly IAuthAdapter _adapter;
    readonly ILogger<SignInQuery> _logger;

    public SignInQueryHandler(IAuthAdapter adapter, ILogger<SignInQuery> logger)
    {
        _adapter = adapter;
        //    _context = context;
        _logger = logger;
    }

    public async Task<SignInModel> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _adapter.SignIn(request.email, request.password);

        return userInfo;
    }
}

