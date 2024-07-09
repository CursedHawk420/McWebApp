using Microsoft.AspNetCore.Identity;

namespace Highgeek.McWebApp.Common.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
#pragma warning disable IDE1006 // Styly pojmenování
        public string? mcUUID { get; set; }
        public string? mcNickname { get; set; }
#pragma warning restore IDE1006 // Styly pojmenování
        public string? SkinHeadPicture { get; set; }

    }
}
