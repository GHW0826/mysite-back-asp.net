using Microsoft.AspNetCore.Authorization;

namespace Water.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AllowAnonymousAttritue : Attribute, IAllowAnonymous
{
}

