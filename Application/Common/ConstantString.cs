
namespace Application.Common;


public enum ShardType
{
    Common = 0,
    Auth = 1
}

public class ConstantString
{
    public static string AUTH_APPSETTING_KEY    = "MysqlContext";
    public static string COMMON_APPSETTING_KEY  = "ShardInfoContext";

    public static string AUTH_SHARD_INFO_KEY    = "auth:shardMange:";
    public static string AUTH_USER_INFO_KEY     = "auth:shardMange:";

    public static string GetAuthShardKey(long number)
    {
        return ConstantString.AUTH_SHARD_INFO_KEY + number.ToString();
    }
}
