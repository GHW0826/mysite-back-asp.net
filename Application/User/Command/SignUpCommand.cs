using Application.Auth.Model;
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Application.User.Model;
using Domain.Entity;
using Domain.Entity.Common;
using MediatR;
using Water.Common.Exceptions;

namespace Application.User.Command;


/// <summary>
/// Sign Up 계정 추가
/// </summary>
public class SignUpCommand : IRequest<SignUpResponseModel>
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
}

/// <summary>
/// SignUpCommand 를 처리합니다.
/// </summary>
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponseModel>
{
    private readonly ShardKeyHelper _shardKeyHelper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IShardInfoContextPool _shardInfoContextPool;
    private readonly IUserContextPool _userContextPool;
    private readonly IAuthTokenContextPool _authTokenContextPool;
    private readonly IDistributedLockContext _distributedLockContext;

    public SignUpCommandHandler(
        ShardKeyHelper shardKeyHelper,
        ITokenHelper tokenHelper,
        IShardInfoContextPool shardInfoContextPool,
        IUserContextPool userContextPool,
        IAuthTokenContextPool authTokenContextPool,
        IDistributedLockContext distributedLockContext)
    {
        _shardKeyHelper = shardKeyHelper;
        _tokenHelper = tokenHelper;
        _shardInfoContextPool = shardInfoContextPool;
        _userContextPool = userContextPool;
        _authTokenContextPool = authTokenContextPool;
        _distributedLockContext = distributedLockContext;
    }

    public async Task<SignUpResponseModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // shard info process
        var shardNumber = _shardKeyHelper.GetShardInfoNumber(request.email);
        var shardInfo = await _shardInfoContextPool.findByEmail(request.email, shardNumber);
        if (shardInfo != null)
            throw new Exception("exist user");

        var newUserId = IdGeneratorHelper.GenerateId();
        UserEntity newUser = UserEntity.Gen(
                newUserId,
                request.email,
                HashHelper.EncryptPassword(request.password),
                request.name
                );

        var newShardInfoId = IdGeneratorHelper.GenerateId();
        ShardInfoEntity newShardInfo = ShardInfoEntity.Gen(
                newShardInfoId,
                request.email,
                newUserId
                );

        // save token info
        var authTokenShardNumber = _shardKeyHelper.GetAuthTokenShardNumber(newUserId);
        TokenInfo tokenInfo = TokenInfo.Gen(
                newUserId,
                request.email,
                request.name,
                request.userRole
            );
        var accessToken = _tokenHelper.GenerateAccessToken(tokenInfo);
        var refreshToken = _tokenHelper.GenerateRefreshToken(tokenInfo);
        AuthTokenEntity newAuthToken = AuthTokenEntity.Gen(
               newUserId,
               accessToken, JWTHelper.GetDefaultAccessTokenExpireTime(),
               refreshToken, JWTHelper.GetDefaultRefreshTokenExpireTime()
            );

        bool locked = await _distributedLockContext.AqurieLock(request.email, "1");
        if (!locked)
            throw new AuthorizationException("exist emailttt");
        
        // user info process
        var userShardNumber = _shardKeyHelper.GetUserShardNumber(newUserId);
        var checkEmail = await _userContextPool.findByEmail(request.email, userShardNumber);
        if (checkEmail != null)
            throw new AuthorizationException("exist email23");

        await _distributedLockContext.ReleaseLock(request.email, "1");

        // save
        await _shardInfoContextPool.save(newShardInfo, shardNumber);
        await _authTokenContextPool.save(newAuthToken, authTokenShardNumber);
        var result = await _userContextPool.save(newUser, userShardNumber);

        return new SignUpResponseModel
        {
            id = result.id,
            email = result.email,
            name = result.name
        };
    }
}
