using Domain.Entity;

namespace Application.Interface;

public interface IAuthTokenContext
{
    public Task ChangesAsync();

    public Task<AuthTokenEntity> save(AuthTokenEntity tokenInfo);

    public Task<AuthTokenEntity?> findByUserId(long id);

    public Task<AuthTokenEntity?> findByIdAndAccessTokenAndRefreshToken(long id, string accessToken, string refreshToken);
}
