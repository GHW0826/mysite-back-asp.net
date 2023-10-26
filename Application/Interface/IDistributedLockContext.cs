using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface;

public interface IDistributedLockContext
{
    public Task<bool> AqurieLock(string key, string value);
    public Task<bool> ReleaseLock(string key, string value);
}
