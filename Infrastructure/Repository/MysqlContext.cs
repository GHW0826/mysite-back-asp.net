using Application.Test;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entity;

namespace Infrastructure.Repository
{
    public class MysqlContext : DbContext, ITestDbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<TestEntity> TestEntitys => Set<TestEntity>();
    }
}
