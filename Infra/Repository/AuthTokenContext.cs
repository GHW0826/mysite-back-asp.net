using Application.Interface;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Infra.Repository;

public class AuthTokenContext : DbContext, IAuthTokenContext
{
    public DbSet<AuthTokenEntity> _authTokenEntities => Set<AuthTokenEntity>();

    public AuthTokenContext() { }

    public AuthTokenContext(DbContextOptions options)
        : base(options)
    { }

    public AuthTokenContext(string connectionString)
        : base(GetOptions(connectionString))
    { }

    public AuthTokenContext(DbContextOptions<AuthTokenContext> options)
    : base(options)
    { }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return MySQLDbContextOptionsExtensions.UseMySQL(new DbContextOptionsBuilder(), connectionString).Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthTokenEntity>(
            b =>
            {
                b.ToTable("tbl_auth_token");
                b.HasKey(c => new { c.user_id });
                b.Property(c => c.access_token).HasColumnName("access_token");
                b.Property(c => c.access_token_expire_time).HasColumnName("access_token_expire_time");
                b.Property(c => c.access_token_update_time).HasColumnName("access_token_update_time");
                b.Property(c => c.refresh_token).HasColumnName("refresh_token");
                b.Property(c => c.refresh_token_expire_time).HasColumnName("refresh_token_expire_time");
                b.Property(c => c.refresh_token_update_time).HasColumnName("refresh_token_update_time");
            });
    }



    public async Task<AuthTokenEntity?> findByIdAndAccessTokenAndRefreshToken(long id, string accessToken, string refreshToken)
    {
        var result = await _authTokenEntities
                            .Where(b => b.user_id == id)
                            .Where(b => b.access_token == accessToken)
                            .Where(b => b.refresh_token == refreshToken)
                            .FirstOrDefaultAsync() ?? null;
        return result;
    }

    public async Task<AuthTokenEntity?> findByUserId(long id)
    {
        var result = await _authTokenEntities
                            .Where(b => b.user_id == id)
                            .FirstOrDefaultAsync() ?? null;
        return result;
    }

    public async Task<AuthTokenEntity> save(AuthTokenEntity tokenInfo)
    {
        var result = await _authTokenEntities.AddAsync(tokenInfo);
        await SaveChangesAsync();
        return result.Entity;
    }
    public async Task ChangesAsync()
    {
        await SaveChangesAsync();
    }
}
