using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Highgeek.McWebApp.BlazorServer.Components.Account
{
    internal sealed class IdentityUserAccessor (UserManager<ApplicationUser> userManager)
    {
        public async Task<ApplicationUser> GetUserAsync(AuthenticationState state)
        {
            var user = await userManager.GetUserAsync(state.User);

            return user ?? throw new ApplicationException("Account / InvalidUser");
        }
    }
}
