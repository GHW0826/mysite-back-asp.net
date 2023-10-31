using Application.Auth.Model;
using Application.Common;
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Domain.Entity;
using Infrastructure.Shard;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Command;


/// <summary>
/// Sign Up 계정 추가
/// </summary>
public class SignUpCommand : IRequest<SaveModel>
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}

/// <summary>
/// SignUpCommand 를 처리합니다.
/// </summary>
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SaveModel>
{
    private readonly IDistributedLockContext _distrulableLockContext;

    private readonly ShardNumberHelper _shardNumberHelper;
    private readonly IShardContextPoolInterface _shardInfoContextPool;
    private readonly IAuthContextPoolInterface _authInfoContextPool;

    public SignUpCommandHandler(
        IDistributedLockContext distrulableLockContext,
        ShardNumberHelper shardNumberHelper,
        IShardContextPoolInterface shardInfoContextPool,
        IAuthContextPoolInterface authInfoContextPool)
    {
        _distrulableLockContext = distrulableLockContext;
        _shardNumberHelper = shardNumberHelper;
        _shardInfoContextPool = shardInfoContextPool;
        _authInfoContextPool = authInfoContextPool;
    }

    public async Task<SaveModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // shard info process
        var shardNumber = _shardNumberHelper.GetAuthShardManagerNumberFromString(request.email);
        var shardKey = ConstantString.GetAuthShardKey(shardNumber);
        var userIdInfoContext = _shardInfoContextPool.GetContext(shardKey)
            ?? throw new Exception("Auth Shard info Context Error - getted context is Null in SignUp");
        var userIdInfo = await userIdInfoContext.GetShardInfo(request.email);
        if (userIdInfo != null)
            throw new Exception("exist user");

        var newUserId = IdGeneratorHelper.GenerateId();
        UserEntity newUser = UserEntity.Gen(
                newUserId, 
                request.email, 
                request.password, /* HashHelper.EncryptPassword(request.Password), */
                request.name);


        // user info process
        var userInfoShardNumber = _shardNumberHelper.GetCommonShardManagerNumberFromId(newUserId);
        var userInfoShardKey = ConstantString.GetAuthShardKey(userInfoShardNumber);
        var userInfoContext = _authInfoContextPool.GetContext(userInfoShardKey)
            ?? throw new Exception("Auth User info Context Error - getted context is Null");
        var userInfo = await userInfoContext.findByEmail(request.email);
        if (userInfo != null)
            throw new Exception("exist user");

        var newShardInfoId = IdGeneratorHelper.GenerateId();
        ShardInfoEntity newShardInfo = ShardInfoEntity.Gen(
                newShardInfoId,
                request.email,
                newUserId);

        // save
        var shardInforesult = await userIdInfoContext.save(newShardInfo);
        var result = await userInfoContext.save(newUser);

        /*
          var result = await _context.save(newUser);
          bool locked = await _distrulableLockContext.AqurieLock(request.Email, "1");
          if (!locked)
              throw new AuthorizationException("exist emailttt");
         
          var checkEmail = await _context.findByEmail(request.Email);
          if (checkEmail != null)
              throw new AuthorizationException("exist email23");
         
          UserEntity newUser = UserEntity.Gen(IdGeneratorHelper.GenerateId(), request.Email, HashHelper.EncryptPassword(request.Password), request.Name);
          var result = await _context.save(newUser);
         
          await _distrulableLockContext.ReleaseLock(request.Email, "1");
        */
        return new SaveModel
        {
            Id = result.id,
            Email = result.email,
            Name = result.name
        };
    }
}
