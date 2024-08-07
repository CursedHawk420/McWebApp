using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Permissions;
using Highgeek.McWebApp.Common.Permissions.Requirements;
using System.Globalization;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Identity;
using Highgeek.McWebApp.Common.Models;
using Microsoft.Extensions.Logging;

namespace Highgeek.McWebApp.Common.Permissions
{

    public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsAuthorizeAttribute>
    {
        private readonly ILogger<PermissionsAuthorizationHandler> _logger;

        private readonly LuckPermsService _luckPermsService;

        private readonly UserService _userService;

        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionsAuthorizationHandler(ILogger<PermissionsAuthorizationHandler> logger, LuckPermsService luckPermsService, UserService userService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _luckPermsService = luckPermsService;
            _userService = userService;
            _userManager = userManager;
        }

        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular
        // context.
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizeAttribute requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization
            // policies(and requirements/handlers) are in use.
            _logger.LogWarning("Evaluating authorization requirement for permission \"" + requirement.Permission + "\"");

            _userService.ApplicationUser = await _userManager.GetUserAsync(context.User);



            if (_userService.ApplicationUser is not null)
            {
                switch (requirement.Permission)
                {
                    case "connectedaccount":

                        if (_userService.ApplicationUser.mcUUID != null)
                        {
                            context.Succeed(requirement);
                            return;
                        }
                        break;


                    case "disconnectedaccount":
                        if (_userService.ApplicationUser.mcUUID == null)
                        {
                            context.Succeed(requirement);
                            return;
                        }
                        break;


                    default:
                        if(_userService.ApplicationUser.mcUUID is not null)
                        {
                            if (await _luckPermsService.HasPermissionAsync(requirement.Permission, _userService.ApplicationUser.mcUUID))
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }
                        else
                        {
                            if (await _luckPermsService.CheckDefaultPerms(requirement.Permission))
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }
                        break;
                }
            }
            else
            {
                if (await _luckPermsService.CheckDefaultPerms(requirement.Permission))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            return;
        }
    }
}
