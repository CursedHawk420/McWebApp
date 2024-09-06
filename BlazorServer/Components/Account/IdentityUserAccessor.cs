using Microsoft.AspNetCore.Identity;
using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Highgeek.McWebApp.BlazorServer.Components.Account;

internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }

    public async Task<ApplicationUser> GetUserAsync(AuthenticationState state)
    {
        var user = await userManager.GetUserAsync(state.User);

        return user ?? throw new ApplicationException("Account / InvalidUser");
    }
}
