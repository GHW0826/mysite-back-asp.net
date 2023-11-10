namespace Domain.Entity.Common;

public class ShardInfoEntity : Entity
{
    public long id { get; set; }

    public string email { get; set; } = string.Empty;
    public long userId { get; set; }

    public static ShardInfoEntity Gen(long id, string email, long userId)
    {
        return new ShardInfoEntity
        {
            id = id,
            email = email,
            userId = userId
        };
    }
}
