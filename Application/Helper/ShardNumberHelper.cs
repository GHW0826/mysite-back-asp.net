using Application.Common;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Infrastructure.Shard;

public interface ShardNumberStrategyInterface
{
}

public class ShardNumberHelper : ShardNumberStrategyInterface
{
    private readonly IConfiguration _config;
    private readonly Dictionary<ShardType, int> _shardCnt = new Dictionary<ShardType, int>();

    public ShardNumberHelper(IConfiguration config)
    {
        _config = config;
        var connectionStrings = config?.GetSection("ConnectionStrings");
        if (connectionStrings != null)
        {
            var commonCnt = connectionStrings.GetChildren()
                            .Where(b => b.Key.StartsWith(ConstantString.COMMON_APPSETTING_KEY))
                            .SelectMany(subSection => subSection.GetChildren())
                            .ToList().Count();
            _shardCnt.Add(ShardType.Common, commonCnt);

            var authShardCnt = connectionStrings.GetChildren()
                .Where(b => b.Key.StartsWith(ConstantString.AUTH_APPSETTING_KEY))
                .SelectMany(subSection => subSection.GetChildren())
                .ToList().Count();
            _shardCnt.Add(ShardType.Auth, authShardCnt);

            var t = 2;
        }
    }

    public int GetAuthShardManagerNumberFromString(string stringkey)
    {
        return GetShardNumberFromString(stringkey, ShardType.Auth);
    }

    public int GetCommonShardManagerNumberFromId(long id)
    {
        return GetShardNumberFromId(id, ShardType.Common);
    }

    public int GetShardNumberFromString(string stringkey, ShardType type)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(stringkey));
        var idx = BitConverter.ToUInt16(hash, 0) % _shardCnt[type];
        return idx;
    }

    public int GetShardNumberFromId(long id, ShardType type)
    {
        var idx = (int) id % _shardCnt[type];
        return idx;
    }
}
