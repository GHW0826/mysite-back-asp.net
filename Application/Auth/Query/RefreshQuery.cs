using Application.Auth.Model;
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Auth.Query;

public class RefreshQuery : IRequest<RefreshResponseModel>
{
    public long id { get; set; }
    public string accessToken { get; set; } = string.Empty;
    public string refreshToken { get; set; } = string.Empty;
}

public class RefreshQueryHandler : IRequestHandler<RefreshQuery, RefreshResponseModel>
{
    private readonly ILogger<RefreshQuery> _logger;
    private readonly IAuthTokenContextPool _authTokenContextPool;
    private readonly ShardKeyHelper _shardKeyHelper;
    private readonly ITokenHelper _tokenHelper;

    public RefreshQueryHandler(ILogger<RefreshQuery> logger, 
        IAuthTokenContextPool authContextPool, 
        ShardKeyHelper shardKeyHelper,
        ITokenHelper tokenHelper)
    {
        _logger = logger;
        _authTokenContextPool = authContextPool;
        _shardKeyHelper = shardKeyHelper;
        _tokenHelper = tokenHelper;
    }

    public async Task<RefreshResponseModel> Handle(RefreshQuery request, CancellationToken cancellationToken)
    {
        var userShardNumber = _shardKeyHelper.GetUserShardNumber(request.id);
        var authTokenContext = _authTokenContextPool.GetContext(userShardNumber);

        var result = await authTokenContext.findByIdAndAccessTokenAndRefreshToken(request.id, request.accessToken, request.refreshToken)
            ?? throw new Exception("xxxx");

        if (result.IsRefreshTokenExpired())
           throw new Exception("refresh token is expired");

        if (result.IsAccessTokenExpired())
        {
            var claims = _tokenHelper.ExtractClaimsFromJwt(request.accessToken);
            if (long.TryParse(claims["id"], out long longValue) == false)
                throw new Exception("invalid id in jwt claims");

            TokenInfo tokenInfo = TokenInfo.Gen(longValue, claims["email"], claims["name"], claims["userRole"]);
            var newAccessToken = _tokenHelper.GenerateAccessToken(tokenInfo);
            result.ChangeAccessToken(newAccessToken, JWTHelper.GetDefaultAccessTokenExpireTime());
            await authTokenContext.ChangesAsync();
        }

        return new RefreshResponseModel
        {
            id =            result.user_id,
            accessToken =   result.access_token
        };
    }
}