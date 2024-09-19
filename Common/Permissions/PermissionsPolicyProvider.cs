using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Permissions.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Highgeek.McWebApp.Common.Permissions
{
    public class PermissionsPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
                                FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
                                FallbackPolicyProvider.GetFallbackPolicyAsync();
        // </snippet_1>

        // <snippet_2>
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            try
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

                policy.AddRequirements(new PermissionsAuthorizeAttribute(policyName));

                var authorizePolicy = policy.Build();

                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }
            catch (Exception ex)
            {
                ex.WriteExceptionToRedis();
                return Task.FromResult<AuthorizationPolicy?>(null);
            }
        }
    }
}
