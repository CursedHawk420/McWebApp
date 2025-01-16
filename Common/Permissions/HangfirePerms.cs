using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Permissions
{
    public class HangfirePerms : IDashboardAuthorizationFilter
    {

        private string policyName;

        public HangfirePerms(string policyName)
        {
            this.policyName = policyName;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            return authService.AuthorizeAsync(httpContext.User, this.policyName).ConfigureAwait(false).GetAwaiter().GetResult().Succeeded;
        }
    }
}
