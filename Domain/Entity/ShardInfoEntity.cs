
namespace Domain.Entity;

public class ShardInfoEntity : Entity
{
    public long id { get; set; }

    public string shardKey { get; set; } = string.Empty;
    public long userId { get; set; }

    public static ShardInfoEntity Gen(long id, string shardKey, long userId)
    {
        return new ShardInfoEntity { 
            id = id,
            shardKey = shardKey,
            userId = userId
        };
    }
}
