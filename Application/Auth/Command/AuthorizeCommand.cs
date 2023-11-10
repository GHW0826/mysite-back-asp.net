using Application.Auth.Model;
using Application.Auth.Query;
using Application.Helper;
using Application.Interface.Sharding;
using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entity;

namespace Application.Auth.Command;

/// <summary>
/// Authorize
/// </summary>
public class AuthorizeCommand : IRequest<AuthorizeResponseModel>
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}

/// <summary>
/// AuthorizeCommand 를 처리.
/// </summary>
public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, AuthorizeResponseModel>
{
    private readonly ILogger<AuthorizeCommand> _logger;
    private readonly ShardKeyHelper _shardKeyHelper;
    private readonly ITokenHelper _tokenHelper;

    private readonly IMapper _mapper;
    private readonly IShardInfoContextPool _shardInfoContextPool;
    private readonly IUserContextPool _userContextPool;
    private readonly IAuthTokenContextPool _authTokenContextPool;

    public AuthorizeCommandHandler(
        ILogger<AuthorizeCommand> logger,
        ShardKeyHelper shardKeyHelper,
        ITokenHelper tokenHelper,
        IMapper mapper,
        IShardInfoContextPool shardInfoContextPool,
        IUserContextPool userContextPool,
        IAuthTokenContextPool authTokenContextPool)
    {
        _logger = logger;
        _shardKeyHelper = shardKeyHelper;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _shardInfoContextPool = shardInfoContextPool;
        _userContextPool = userContextPool;
        _authTokenContextPool = authTokenContextPool;
    }

    public async Task<AuthorizeResponseModel> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
    {
        // shard info process
        var shardNumber = _shardKeyHelper.GetShardInfoNumber(request.email);
        var shardInfo = await _shardInfoContextPool.findByEmail(request.email, shardNumber)
                                ?? throw new Exception("invalid user");

        // user info check
        var userShardNumber = _shardKeyHelper.GetUserShardNumber(shardInfo.userId);
        var userInfo = await _userContextPool.findByEmailAndPassword(
            request.email,
            HashHelper.EncryptPassword(request.password),
            userShardNumber) ?? throw new Exception("invalid user");

        // token store
        var authTokenContext = _authTokenContextPool.GetContext(userShardNumber);
        var authTokenEntity = await authTokenContext.findByUserId(userInfo.id);
        // var authTokenEntity = await _authTokenContextPool.findByUserId(userInfo.id, userShardNumber);
        var tokenInfo = _mapper.Map<TokenInfo>(userInfo) ?? throw new Exception("fail convert entity to token");

        var accessToken = "";
        var refreshToken = "";
        // first token
        if (authTokenEntity == null)
        {
            accessToken = _tokenHelper.GenerateAccessToken(tokenInfo);
            refreshToken = _tokenHelper.GenerateRefreshToken(tokenInfo);
            authTokenEntity = AuthTokenEntity.Gen(
                userInfo.id,
                accessToken, JWTHelper.GetDefaultAccessTokenExpireTime(),
                refreshToken, JWTHelper.GetDefaultRefreshTokenExpireTime()
                );
            await authTokenContext.save(authTokenEntity);
        }
        else
        {
            accessToken = authTokenEntity.access_token;
            refreshToken = authTokenEntity.refresh_token;

            // token info update
            if (authTokenEntity.IsAccessTokenExpired())
            {
                accessToken = _tokenHelper.GenerateAccessToken(tokenInfo);
                authTokenEntity.ChangeAccessToken(accessToken, JWTHelper.GetDefaultAccessTokenExpireTime());
            }
            if (authTokenEntity.IsRefreshTokenExpired())
            {
                refreshToken = _tokenHelper.GenerateRefreshToken(tokenInfo);
                authTokenEntity.ChangeRefreshToken(refreshToken, JWTHelper.GetDefaultRefreshTokenExpireTime());
            }
            await authTokenContext.ChangesAsync();
        }

        // await _authTokenContextPool.save(authTokenEntity, userShardNumber);
        return new AuthorizeResponseModel
        {
            id = userInfo.id,
            accessToken = accessToken,
            refreshToken = refreshToken
        };
    }
}