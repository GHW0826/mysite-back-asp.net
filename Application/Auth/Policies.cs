

using Microsoft.AspNetCore.Authorization;

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