using Microsoft.EntityFrameworkCore;
using mysite_back_asp.net.Entity;

namespace mysite_back_asp.net.Cache
{
    public class RedisCache : DbContext
    {

        public DbSet<TestEntity> testEntities => Set<TestEntity>();
    }
}
