using MediatR;
using Microsoft.Extensions.Logging;
using System;
using Water.Common.Attributes;
using Water.Common.Interfaces;

namespace Application.Auth.Query;

/*
[Authorize]
public class GetAuthenticatedQuery : IRequest<bool>
{
}


public class GetAuthenticatedQueryHandler : IRequestHandler<GetAuthenticatedQuery, bool>
{
    //   readonly IDateTime _dateTime;

    readonly IAuthUser _authUser;
    readonly ILogger<GetAuthenticatedQuery> _logger;

    public GetAuthenticatedQueryHandler(IAuthUser loginUser,ILogger<GetAuthenticatedQuery> logger)
    {
        _authUser = loginUser;
       // _dateTime = dateTime;
        _logger = logger;
    }
    public async Task<bool> Handle(GetAuthenticatedQuery request, CancellationToken cancellationToken)
    {
        return _authUser.IsAuthenticated;
    }
}
*/