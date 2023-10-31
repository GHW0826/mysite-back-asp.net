using Application.Auth;
using Application.Interface.Sharding;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AuthContext : DbContext, IAuthDbContext
{
    public DbSet<UserEntity> userEntities => Set<UserEntity>();

    public AuthContext() { }

    public AuthContext(DbContextOptions options)
        : base(options)
    { }

    public AuthContext(string connectionString)
        : base(GetOptions(connectionString))
    {}

    public AuthContext(DbContextOptions<AuthContext> options)
    : base(options)
    { }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return MySQLDbContextOptionsExtensions.UseMySQL(new DbContextOptionsBuilder(), connectionString).Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(
            b =>
            {
                b.ToTable("tb_user");
                b.HasKey(c => new { c.id });
                b.Property(c => c.email).HasColumnName("email");
                b.Property(c => c.password).HasColumnName("password");
                b.Property(c => c.name).HasColumnName("name");
            });
    }

    public async Task<UserEntity> save(UserEntity user)
    {
        var result = await userEntities.AddAsync(user);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<UserEntity?> findByEmailAndPassword(string email, string password)
    {
        var result = await userEntities
                        .Where(b => b.email == email)
                        .Where(b => b.password == password)
                        .FirstOrDefaultAsync() ?? null;
        return result;
    }

    public async Task<UserEntity?> findByEmail(string email)
    {
        var result = await userEntities
                .Where(b => b.email == email)
                .FirstOrDefaultAsync();
        return result;
    }

    public async Task<UserEntity?> findById(long id)
    {
        var result = await userEntities
            .Where(b => b.id == id)
            .FirstOrDefaultAsync();
        return result;
    }
}
