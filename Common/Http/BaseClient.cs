using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Common.Http;

public abstract class BaseClient
{
    private readonly HttpClient _client; 
    
    protected BaseClient(HttpClient client)
    {
        _client = client;
    }
}
