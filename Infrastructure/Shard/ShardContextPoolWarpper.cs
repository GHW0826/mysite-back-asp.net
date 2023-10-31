using Application.Interface.Sharding;
using Infrastructure.Helper;
using Infrastructure.Repository;

namespace Infrastructure.Shard;

public class ShardContextPoolWarpper : IShardContextPoolInterface
{
    private readonly DbContextPoolMultiplexerService<ShardInfoContext> _dbContextPoolMultiplexerService;

    public ShardContextPoolWarpper(DbContextPoolMultiplexerService<ShardInfoContext> dbContextPoolMultiplexerService) 
    {
        _dbContextPoolMultiplexerService = dbContextPoolMultiplexerService;
    }

    public IShardManageContext? GetContext(string name)
    {
        return _dbContextPoolMultiplexerService.GetDbContext(name);
    }
}
