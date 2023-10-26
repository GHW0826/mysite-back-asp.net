using Application.Auth;
using Application.Auth.Model;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AuthContext : DbContext, IAuthDbContext
{
    public DbSet<UserEntity> userEntities => Set<UserEntity>();

    public AuthContext(DbContextOptions<AuthContext> options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(
            b =>
            {
                b.ToTable("tb_user");
                b.HasKey(c => new { c.Id });
                b.Property(c => c.Email).HasColumnName("email");
                b.Property(c => c.Password).HasColumnName("password");
            });
    }

    public async Task<UserEntity?> findByEmailAndPassword(string email, string password)
    {
        var result = await userEntities
                        .Where(b => b.Email == email)
                        .Where(b => b.Password == password)
                        .FirstOrDefaultAsync() ?? null;
        if (result == null)
            return null;

        return result;
    }

    public async Task<UserEntity?> findByEmail(string email)
    {
        var result = await userEntities
                .Where(b => b.Email == email)
                .FirstOrDefaultAsync();
        return result;
    }

    public async Task<UserEntity?> findById(ulong id)
    {
        var result = await userEntities
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<UserEntity> save(UserEntity user)
    {
        var result = await userEntities.AddAsync(user);
        SaveChanges();
        return result.Entity;
    }
}
