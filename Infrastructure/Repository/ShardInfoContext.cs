
using Application.Auth;
using Application.Interface;
using Domain.Entity.Common;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Infrastructure.Repository;

public class ShardInfoContext : DbContext, IShardInfoContext
{
    public DbSet<ShardInfoEntity> _shardInfoEntities => Set<ShardInfoEntity>();

    public ShardInfoContext() { }

    public ShardInfoContext(DbContextOptions options)
        :base (options)
    { }
        public ShardInfoContext(string connectionString)
        : base(GetOptions(connectionString))
    { }

    public ShardInfoContext(DbContextOptions<ShardInfoContext> options)
    : base(options)
    { }
    private static DbContextOptions GetOptions(string connectionString)
    {
        return MySQLDbContextOptionsExtensions.UseMySQL(new DbContextOptionsBuilder(), connectionString).Options;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShardInfoEntity>(
            b =>
            {
                b.ToTable("tbl_shard_info");
                b.HasKey(c => new { c.id });
                b.Property(c => c.email).HasColumnName("email");
                b.Property(c => c.userId).HasColumnName("user_id");
            });
    }

    public async Task<ShardInfoEntity?> GetShardInfo(string email)
    {
        var result = await _shardInfoEntities
                            .Where(b => b.email == email)
                            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<ShardInfoEntity?> findByEmail(string email)
    {
        var result = await _shardInfoEntities
                            .Where(b => b.email == email)
                            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<ShardInfoEntity> save(ShardInfoEntity shardInfo)
    {
        var result = await _shardInfoEntities.AddAsync(shardInfo);
        await SaveChangesAsync();
        return result.Entity;
    }
}
