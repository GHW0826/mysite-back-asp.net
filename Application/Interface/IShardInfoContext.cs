using Domain.Entity.Common;

namespace Application.Interface;

public interface IShardInfoContext
{
    public Task<ShardInfoEntity> save(ShardInfoEntity shardInfo);
    public Task<ShardInfoEntity?> GetShardInfo(string shardKey);
    public Task<ShardInfoEntity?> findByEmail(string email);

}
