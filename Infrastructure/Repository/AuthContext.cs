using Application.Auth;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AuthContext : DbContext, IAuthDbContext
{
    public DbSet<SignUpEntity> signUpEntities => Set<SignUpEntity>();

    public AuthContext(DbContextOptions<AuthContext> options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SignUpEntity>(
            b =>
            {
                b.HasKey(c => new { c.id });
                b.Property(c => c.email).HasColumnName("email");
                b.Property(c => c.password).HasColumnName("password");
                b.ToTable("TB_USER");
            });
    }

    public async Task<SignUpEntity?> findByEmailAndPassword(string email, string password)
    {
        var result = await signUpEntities
                        .Where(b => b.email == email)
                        .Where(b => b.password == password)
                        .FirstOrDefaultAsync() ?? null;
        return result;
    }
}
