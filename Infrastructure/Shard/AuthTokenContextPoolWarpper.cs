using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Application.User;
using Domain.Entity;
using Infrastructure.Helper;
using Infrastructure.Repository;

namespace Infrastructure.Shard;

public class AuthTokenContextPoolWarpper : IAuthTokenContextPool
{
    private readonly DbContextPoolMultiplexerService<AuthTokenContext> _dbContextPoolMultiplexerService;

    public AuthTokenContextPoolWarpper(DbContextPoolMultiplexerService<AuthTokenContext> dbContextPoolMultiplexerService)
    {
        _dbContextPoolMultiplexerService = dbContextPoolMultiplexerService;
    }

    public IAuthTokenContext GetContext(string name)
    {
        return _dbContextPoolMultiplexerService.GetDbContext(name) ?? throw new Exception("User Context is null");
    }

    public IAuthTokenContext GetContext(long shardNumber)
    {
        string shardKey = ShardKeyHelper.AUTH_TOKEN_CONTEXT_KEY + shardNumber.ToString();
        return GetContext(shardKey);
    }

    public async Task<AuthTokenEntity> save(AuthTokenEntity authToken, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return await context.save(authToken);
    }

    public async Task<AuthTokenEntity?> findByUserId(long id, int shardNumber)
    {
        string shardKey = ShardKeyHelper.AUTH_TOKEN_CONTEXT_KEY + shardNumber.ToString();
        var context = GetContext(shardKey);
        return await context.findByUserId(id);
    }

    //////////////////////// No Sharding 용 TODO 개발 (Dont Touch) ////////////////////////
    private async Task<AuthTokenEntity?> findByUserId(long id)
    {
        var context = GetContext() ?? throw new Exception("User Context is null"); ;
        return await context.findByUserId(id);
    }
    private IAuthTokenContext? GetContext()
    {
        return null;
    }
}
