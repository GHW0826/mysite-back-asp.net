using Application.Auth;
using Domain.Entity;

namespace Application.Interface.Sharding;

public interface IAuthContextPoolInterface
{
    public IAuthDbContext? GetContext(string name);
}
