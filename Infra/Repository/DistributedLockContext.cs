using Application.Interface;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Infra.Repository;

public class DistributedLockContext : IDistributedLockContext
{
    private readonly IConfiguration _config;
    private readonly IDatabaseAsync _context;

    public DistributedLockContext(IDatabaseAsync context, IConfiguration config)
    {
        _context = context; 
        _config = config;
    }
 
    public async Task<bool> AqurieLock(string key, string value)
    {
        bool flag = false;
        flag = await _context.LockTakeAsync(key, value, new TimeSpan(0, 0, 100));

        return flag;
    }

    public async Task<bool> ReleaseLock(string key, string value)
    {
        bool flag = false;
        flag = await _context.LockReleaseAsync(key, value);

        return flag;
    }
}
