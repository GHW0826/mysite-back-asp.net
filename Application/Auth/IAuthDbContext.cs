

using Application.Auth.Model;
using Domain.Entity;

namespace Application.Auth;

public interface IAuthDbContext
{
    public Task<UserEntity> save(UserEntity user);

    public Task<UserEntity?> findById(ulong id);


    public Task<UserEntity?> findByEmailAndPassword(string email, string password);

    public Task<UserEntity?> findByEmail(string email);
}
