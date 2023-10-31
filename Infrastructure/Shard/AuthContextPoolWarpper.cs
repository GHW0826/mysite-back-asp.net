using Application.Auth;
using Application.Interface.Sharding;
using Infrastructure.Helper;
using Infrastructure.Repository;

namespace Infrastructure.Shard;

public class AuthContextPoolWarpper : IAuthContextPoolInterface
{
    private readonly DbContextPoolMultiplexerService<AuthContext> _dbContextPoolMultiplexerService;

    public AuthContextPoolWarpper(DbContextPoolMultiplexerService<AuthContext> dbContextPoolMultiplexerService) 
    {
        _dbContextPoolMultiplexerService = dbContextPoolMultiplexerService;
    }

    public IAuthDbContext? GetContext(string name)
    {
        return _dbContextPoolMultiplexerService.GetDbContext(name);
    }
}
