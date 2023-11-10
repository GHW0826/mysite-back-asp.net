using Domain.Entity;

namespace Application.Interface.Sharding;

public interface IUserContextPool
{
    public IUserContext? GetContext(string name);
    public Task<UserEntity> save(UserEntity user, int shardNumber);
    public Task<UserEntity?> findByEmail(string email, int shardNumber);
    public Task<UserEntity?> findByEmailAndPassword(string email, string password, int shardNumber);
}
