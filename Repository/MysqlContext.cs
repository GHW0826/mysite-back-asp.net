using Microsoft.EntityFrameworkCore;
using mysite_back_asp.net.Entity;

namespace mysite_back_asp.net.Repository
{
    public class MysqlContext : DbContext
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
