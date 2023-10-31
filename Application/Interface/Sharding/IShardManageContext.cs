using Domain.Entity;

namespace Application.Interface.Sharding;

public interface IShardManageContext
{
    public Task<ShardInfoEntity?> GetShardInfo(string shardKey);

    public Task<ShardInfoEntity> save(ShardInfoEntity shardInfo);
}
