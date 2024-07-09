namespace Highgeek.McWebApp.Common.Models.Minecraft
{
    public class GameItem
    {
        public string name { get; set; }
        public string Json { get; set; }
        public string? Texture { get; set; }
        public string? TextureName { get; set; }
        public int? Amount { get; set; }
        public Dictionary<string, int>? Enchantments { get; set; }
        public string? Identifier { get; set; }
        public string? OriginUuid { get; set; }
        public string? DisplayName { get; set; }
    }
}
