using System.Security.Claims;

namespace CommuteCalculator.Extensions;

public static class ClaimExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        return Guid.Parse(principal.Claims.Single(x => x.Type == "UserId").Value);
    }
}