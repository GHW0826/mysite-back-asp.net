using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace mysite_back_asp.net.Cache;

public class RedisCache : DbContext
{

    public DbSet<TestEntity> testEntities => Set<TestEntity>();
}
