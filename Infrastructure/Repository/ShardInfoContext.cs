
using Application.Auth;
using Application.Interface.Sharding;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Infrastructure.Repository;

public class ShardInfoContext : DbContext, IShardManageContext
{
    public DbSet<ShardInfoEntity> shardInfoEntities => Set<ShardInfoEntity>();

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
                b.ToTable("tb_shard_info");
                b.HasKey(c => new { c.id });
                b.Property(c => c.shardKey).HasColumnName("shard_key");
                b.Property(c => c.userId).HasColumnName("user_id");
            });
    }

    public async Task<ShardInfoEntity?> GetShardInfo(string shardKey)
    {
        var result = await shardInfoEntities
                            .Where(b => b.shardKey == shardKey)
                            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<ShardInfoEntity> save(ShardInfoEntity shardInfo)
    {
        var result = await shardInfoEntities.AddAsync(shardInfo);
        await SaveChangesAsync();
        return result.Entity;
    }
}
