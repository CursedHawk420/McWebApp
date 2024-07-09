using Microsoft.AspNetCore.Authorization;

namespace Highgeek.McWebApp.Common.Permissions.Requirements
{
    public class PermissionsAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        public PermissionsAuthorizeAttribute(string perm) => Permission = perm;
        public string Permission { get; set; }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }
    }
}
