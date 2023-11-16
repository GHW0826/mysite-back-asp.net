
using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Domain.Entity;
using Infra.Repository;
using Infrastructure.Helper;

namespace Infra.Shard;

public class UserContextPoolWarpper : IUserContextPool
{
    private readonly DbContextPoolService<UserContext> _dbContextPoolMultiplexerService;

    public UserContextPoolWarpper(DbContextPoolService<UserContext> dbContextPoolMultiplexerService)
    {
        _dbContextPoolMultiplexerService = dbContextPoolMultiplexerService;
    }

    public IUserContext GetContext(string name)
    {
        return _dbContextPoolMultiplexerService.GetDbContext(name) ?? throw new Exception("User Context is null");
    }

    public IUserContext GetContext(long shardNumber)
    {
        string shardKey = ShardKeyHelper.USER_CONTEXT_KEY + shardNumber.ToString();
        return GetContext(shardKey);
    }

    public async Task<UserEntity> save(UserEntity user, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return await context.save(user);
    }

    public async Task<UserEntity?> findByEmail(string email, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return await context.findByEmail(email);
    }

    public async Task<UserEntity?> findByEmailAndPassword(string email, string password, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return await context.findByEmailAndPassword(email, password);
    }

    //////////////////////// No Sharding 용 TODO 개발 (Dont Touch) ////////////////////////
    private async Task<UserEntity?> findByEmail(string email)
    {
        var context = GetContext() ?? throw new Exception("User Context is null"); ;
        return await context.findByEmail(email);
    }
    private IUserContext? GetContext()
    {
        return null;
    }
}
