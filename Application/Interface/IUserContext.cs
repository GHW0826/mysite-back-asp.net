using Domain.Entity;

namespace Application.Interface;

public interface IUserContext
{
    public Task<UserEntity> save(UserEntity user);

    public Task<UserEntity?> findById(long id);

    public Task<UserEntity?> findByEmailAndPassword(string email, string password);

    public Task<UserEntity?> findByEmail(string email);
}
