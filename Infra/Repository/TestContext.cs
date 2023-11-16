
using Application.Test;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class TestContext : DbContext, ITestDbContext
{
    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public DbSet<TestEntity> TestEntitys => Set<TestEntity>();
}
