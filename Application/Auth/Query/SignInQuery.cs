using MediatR;
using Microsoft.Extensions.Logging;
using Application.Auth.Model;
using Application.Helper;
using Microsoft.EntityFrameworkCore;
using Application.Interface.Sharding;
using Infrastructure.Shard;
using System.Text;
using Application.Common;

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
    private readonly ShardNumberHelper _shardNumberHelper;
    private readonly IShardContextPoolInterface _ShardInfoContextPool;
    private readonly IAuthContextPoolInterface _AuthInfoContextPool;

    public SignInQueryHandler(
        ILogger<SignInQueryHandler> logger,
        ShardNumberHelper shardNumberHelper,
        IShardContextPoolInterface ShardInfoContextPool,
        IAuthContextPoolInterface AuthInfoContextPool)
    {
        _logger = logger;
        _shardNumberHelper = shardNumberHelper;
        _ShardInfoContextPool = ShardInfoContextPool;
        _AuthInfoContextPool = AuthInfoContextPool;
    }

    public async Task<SignInModel?> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        // Email로 User Id 획득
        var shardNumber = _shardNumberHelper.GetAuthShardManagerNumberFromString(request.email);
        var shardKey = ConstantString.GetAuthShardKey(shardNumber);
        var userIdInfoContext = _ShardInfoContextPool.GetContext(shardKey) 
            ?? throw new Exception("Auth Shard info Context Error - getted context is Null in SignIn");
        var userIdInfo = await userIdInfoContext.GetShardInfo(request.email);
        if (userIdInfo == null)
            return null;

        // user info check
        var userInfoShardNumber = _shardNumberHelper.GetCommonShardManagerNumberFromId(userIdInfo.userId);
        var userInfoShardKey = ConstantString.GetAuthShardKey(userInfoShardNumber);
        var userInfoContext = _AuthInfoContextPool.GetContext(userInfoShardKey)
            ?? throw new Exception("Auth User info Context Error - getted context is Null");
        var userInfo = await userInfoContext.findByEmailAndPassword(request.email, request.password /* HashHelper.EncryptPassword(request.password) */);
        if (userInfo == null)
            return null;
        
        return new SignInModel {
            id = userInfo.id,
            email = userInfo.email,
            name = userInfo.name
        };
    }
}

