

namespace Highgeek.McWebApp.Common.Models
{
    public class MinecraftUser
    {
        public string Uuid { get; set; }
        public string NickName { get; set; }
        public string ApplicationUserName { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApplicationUserEmail { get; set; }
        public bool IsPremium { get; set; }
        public string? PremiumUuid { get; set; }
        public string? SkinTexture { get; set; }
        public string? SkinHeadTexture { get; set; }
        public string? LpUserGroup { get; set; }
        public byte[]? SkinHeadTextureImage { get; set; }
        public byte[]? EcoId { get; set; }
    }
}
