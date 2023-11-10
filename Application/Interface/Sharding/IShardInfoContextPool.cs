using Domain.Entity.Common;

namespace Application.Interface.Sharding;

public interface IShardInfoContextPool
{
    public IShardInfoContext GetContext(string name);

    public Task<ShardInfoEntity> save(ShardInfoEntity shardInfo, int shardNumber);

    public Task<ShardInfoEntity?> findByEmail(string email, int shardNumber);
}
