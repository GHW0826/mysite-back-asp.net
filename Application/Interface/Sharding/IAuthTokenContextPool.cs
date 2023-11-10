using Domain.Entity;
using Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Sharding;

public interface IAuthTokenContextPool
{
    public IAuthTokenContext GetContext(string key);
    public IAuthTokenContext GetContext(long shardNumber);
    public Task<AuthTokenEntity?> findByUserId(long id, int shardNumber);
    public Task<AuthTokenEntity> save(AuthTokenEntity authToken, int shardNumber);
}
