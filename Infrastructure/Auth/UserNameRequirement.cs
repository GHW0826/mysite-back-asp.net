using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth;

public class UserNameRequirement : IAuthorizationRequirement
{
    public UserNameRequirement(string username)
    {
        this.UserName = username;
    }

    public string UserName { get; set; }
}
