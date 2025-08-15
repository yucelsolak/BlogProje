using System;
using System.Linq;
using System.Security.Claims;

namespace BlogProje.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInAnyRoles(this ClaimsPrincipal user, string csv)
        {
            var roles = csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return roles.Any(user.IsInRole);
        }
    }
}
