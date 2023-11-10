using Application.Helper;
using Application.Interface;
using Application.Interface.Sharding;
using Domain.Entity;
using Domain.Entity.Common;
using Infrastructure.Helper;
using Infrastructure.Repository;

namespace Infrastructure.Shard;

public class ShardInfoContextPoolWarpper : IShardInfoContextPool
{
    private readonly DbContextPoolMultiplexerService<ShardInfoContext> _dbContextPoolMultiplexerService;

    public ShardInfoContextPoolWarpper(DbContextPoolMultiplexerService<ShardInfoContext> dbContextPoolMultiplexerService) 
    {
        _dbContextPoolMultiplexerService = dbContextPoolMultiplexerService;
    }

    public IShardInfoContext GetContext(string name)
    {
        return _dbContextPoolMultiplexerService.GetDbContext(name) ?? throw new Exception("ShardInfoContext is null");
    }
    public IShardInfoContext GetContext(long shardNumber)
    {
        string shardKey = ShardKeyHelper.SHARD_INFO_CONTEXT_KEY + shardNumber.ToString();
        return GetContext(shardKey);
    }

    public async Task<ShardInfoEntity> save(ShardInfoEntity shardInfo, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return await context.save(shardInfo);
    }

    public Task<ShardInfoEntity?> findByEmail(string email, int shardNumber)
    {
        var context = GetContext(shardNumber);
        return context.findByEmail(email);
    }

    //////////////////////// No Sharding 용 TODO 개발 (Dont Touch) ////////////////////////
    private Task<ShardInfoEntity?> findByEmail(string email)
    {
        var context = GetContext() ?? throw new Exception("");
        return context.findByEmail(email);
    }
    private IShardInfoContext? GetContext()
    {
        return null;
    }
}
