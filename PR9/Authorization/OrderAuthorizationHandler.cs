using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PR9.Dtos;

namespace PR9.Authorization;

public class OrderAuthorizationHandler : AuthorizationHandler<OwnerOrAdminRequirement, OrderDTO>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement,
        OrderDTO resource)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId) && userId == resource.UserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
