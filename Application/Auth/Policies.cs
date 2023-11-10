using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth;

public class Policies
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string UserName = "UserName";

    public static AuthorizationPolicy AdminPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
    }

    public static AuthorizationPolicy UserPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
    }

    public static AuthorizationPolicy UserNamePolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireUserName("1").Build();
    }
}