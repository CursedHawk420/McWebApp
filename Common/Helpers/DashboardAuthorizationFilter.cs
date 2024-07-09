using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace Highgeek.McWebApp.Common.Helpers
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //TODO update to blazorserver authorization
            return context.GetHttpContext().User.IsInRole("SuperAdmin");
        }
    }
}
