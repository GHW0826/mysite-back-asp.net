
using Application.Interface;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class UserContext : DbContext, IUserContext
{
    public DbSet<UserEntity> _userEntities => Set<UserEntity>();

    public UserContext() { }

    public UserContext(DbContextOptions options)
        : base(options)
    { }

    public UserContext(string connectionString)
        : base(GetOptions(connectionString))
    {}

    public UserContext(DbContextOptions<UserContext> options)
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
                b.ToTable("tbl_user");
                b.HasKey(c => new { c.id });
                b.Property(c => c.email).HasColumnName("email");
                b.Property(c => c.password).HasColumnName("password");
                b.Property(c => c.name).HasColumnName("name");
                b.Property(c => c.userRole).HasColumnName("user_role");
            });
    }

    public async Task<UserEntity> save(UserEntity user)
    {
        var result = await _userEntities.AddAsync(user);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<UserEntity?> findByEmailAndPassword(string email, string password)
    {
        var result = await _userEntities
                        .Where(b => b.email == email)
                        .Where(b => b.password == password)
                        .FirstOrDefaultAsync() ?? null;
        return result;
    }

    public async Task<UserEntity?> findByEmail(string email)
    {
        var result = await _userEntities
                .Where(b => b.email == email)
                .FirstOrDefaultAsync();
        return result;
    }

    public async Task<UserEntity?> findById(long id)
    {
        var result = await _userEntities
            .Where(b => b.id == id)
            .FirstOrDefaultAsync();
        return result;
    }
}
