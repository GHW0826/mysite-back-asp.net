using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helper;

public enum ShardType
{
    ShardInfo   = 0,
    User        = 1,
    AuthToken   = 2
}

public interface ShardNumberStrategyInterface
{
}

public class ShardKeyHelper : ShardNumberStrategyInterface
{
    public static readonly string SHARD_INFO_APPSETTING_KEY             = "ShardInfoContext";
    public static readonly string USER_APPSETTING_KEY                   = "UserContext";
    public static readonly string AUTH_TOKEN_APPSETTING_KEY             = "AuthTokenContext";

    public static readonly string SHARD_INFO_CONTEXT_KEY                = "ShardInfo:";
    public static readonly string USER_CONTEXT_KEY                      = "User:";
    public static readonly string AUTH_TOKEN_CONTEXT_KEY                = "AuthToken:";

    public static readonly string PREFIX_SHARD_INFO_CONNECTION_STRING   = "ShardInfoContext:Shard";
    public static readonly string PREFIX_USER_CONNECTION_STRING         = "UserContext:Shard";
    public static readonly string PREFIX_AUTH_TOKEN_CONNECTION_STRING   = "AuthTokenContext:Shard";


    private readonly IConfiguration _config;
    private readonly Dictionary<ShardType, int> _shardCnt = new Dictionary<ShardType, int>();


    public ShardKeyHelper(IConfiguration config)
    {
        _config = config;
        var connectionStrings = config?.GetSection("ConnectionStrings");
        if (connectionStrings != null)
        {
            var shardInfoShardCnt = connectionStrings.GetChildren()
                            .Where(b => b.Key.StartsWith(ShardKeyHelper.SHARD_INFO_APPSETTING_KEY))
                            .SelectMany(subSection => subSection.GetChildren())
                            .ToList().Count();
            _shardCnt.Add(ShardType.ShardInfo, shardInfoShardCnt);

            var userShardCnt = connectionStrings.GetChildren()
                .Where(b => b.Key.StartsWith(ShardKeyHelper.USER_APPSETTING_KEY))
                .SelectMany(subSection => subSection.GetChildren())
                .ToList().Count();
            _shardCnt.Add(ShardType.User, userShardCnt);

            var authTokenShardCnt = connectionStrings.GetChildren()
                .Where(b => b.Key.StartsWith(ShardKeyHelper.AUTH_TOKEN_APPSETTING_KEY))
                .SelectMany(subSection => subSection.GetChildren())
                .ToList().Count();
            _shardCnt.Add(ShardType.AuthToken, authTokenShardCnt);
        }
    }

    ////////////////////////////////////////// Number ////////////////////////////////////////////////////
    
    /// <summary>
    /// Get Shard Info Shard Number
    /// </summary>
    /// <param name="key"> email </param>
    /// <returns></returns>
    public int GetShardInfoNumber(string key)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(key));
        var idx = BitConverter.ToUInt16(hash, 0) % _shardCnt[ShardType.ShardInfo];
        return idx;
    }

    /// <summary>
    /// Get User Shard Number
    /// </summary>
    /// <param name="id"> user id </param>
    /// <returns></returns>
    public int GetUserShardNumber(long id)
    {
        var idx = (int)id % _shardCnt[ShardType.User];
        return idx;
    }


    /// <summary>
    /// Get Auth Token Shard Number
    /// </summary>
    /// <param name="id"> user id </param>
    /// <returns></returns>
    public int GetAuthTokenShardNumber(long id)
    {
        var idx = (int)id % _shardCnt[ShardType.AuthToken];
        return idx;
    }

    ////////////////////////////////////////// Key ////////////////////////////////////////////////////

    /// <summary>
    /// Get ShardInfo Context Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetShardInfoContextKey(string key)
    {
        var shardNumber = GetShardInfoNumber(key);
        return SHARD_INFO_CONTEXT_KEY + shardNumber.ToString();
    }

    /// <summary>
    /// Get User Context Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetUserContextKey(long id)
    {
        var shardNumber = GetUserShardNumber(id);
        return USER_CONTEXT_KEY + shardNumber.ToString();
    }

    /// <summary>
    /// Get Auth Token Context Key
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetAuthTokenContextKey(long id)
    {
        var shardNumber = GetAuthTokenShardNumber(id);
        return AUTH_TOKEN_CONTEXT_KEY + shardNumber.ToString();
    }
}
