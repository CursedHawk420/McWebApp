using Highgeek.McWebApp.Common.Models.Minecraft.DisplayName;
using Highgeek.McWebApp.Common.Services.Redis;
using SharpNBT;
using SharpNBT.SNBT;
using System.Globalization;

namespace Highgeek.McWebApp.Common.Models.Minecraft
{
    public class GameItem
    {
        public string? Name { get; set; }
        public string? Json { get; set; }
        public string? Texture { get; set; }
        public string? TextureUrl { get; set; }
        public int? Amount { get; set; }
        public string? Identifier { get; set; }
        public string? OriginUuid { get; set; }
        public DisplayNameAdapter? DisplayName { get; set; }
        public string? CustomName { get; set; }
        public CompoundTag CompoundTag { get; set; }
        public CompoundTag Components { get; set; }
        public CompoundTag? Enchantments { get; set; }
        public ListTag? Modifiers { get; set; }
        public CompoundTag? PublicBukkitValues { get; set; }
        public string? Id { get; set; }


        public static string AIRITEM = "{\r\n    \"id\": \"minecraft:air\"\r\n}";

        public GameItem (string json, string originUuid)
        {
            this.Json = json;
            this.OriginUuid = originUuid;
            try
            {
                CompoundTag = StringNbt.Parse(json);
            }
            catch (Exception ex)
            {
                CompoundTag = StringNbt.Parse("{\r\n    DataVersion: 3955,\r\n    count: 1,\r\n    id: \"minecraft:barrier\"\r\n}");
                RedisService.SetInRedis("asd:asd", ex.Message);
            }
            Amount = GetCount();
            Id = ((StringTag)CompoundTag["id"]).Value;
            Name = Id.ToLower().Substring(Id.IndexOf(":") + 1, Id.Length - Id.IndexOf(":") - 1);
            if (Id != "minecraft:air")
            {
                Identifier = originUuid;
            }

            Components = GetComponents();

            Enchantments = GetEnchantments();
            Modifiers = GetModifiers();
            PublicBukkitValues = GetBukkitValues();
            CustomName = GetCustomName();
            DisplayName = GetDisplayName();
        }

        public DisplayNameAdapter GetDisplayName()
        {
            //todo: from compound tag get item display name
            if (CustomName != null)
            {
                return DisplayNameAdapter.FromJson(CustomName);
            }
            else
            {
                var displayName = new DisplayNameAdapter();
                displayName.HtmlText = Name;
                displayName.HtmlText = WordsToUpper(displayName.HtmlText);
                return displayName;
            }
        }

        public int GetCount()
        {
            int amount;
            try
            {
                amount = CompoundTag.Get<IntTag>("count");
            }
            catch (Exception ex)
            {
                amount = 1;
            }
            return amount;
        }

        public CompoundTag GetComponents()
        {
            if (CompoundTag.ContainsKey("components"))
            {
                return (CompoundTag)CompoundTag["components"];
            }
            else
            {
                return null;
            }
        }

        public ListTag GetModifiers()
        {
            if (Components is not null && Components.ContainsKey("minecraft:attribute_modifiers"))
            {
                CompoundTag modifiersTag = ((CompoundTag)Components["minecraft:attribute_modifiers"]);
                return ((ListTag)modifiersTag["modifiers"]);
            }
            else
            {
                return null;
            }
        }

        public CompoundTag GetBukkitValues()
        {
            if (Components is not null && Components.ContainsKey("minecraft:custom_data"))
            {
                CompoundTag custom_dataTag = ((CompoundTag)Components["minecraft:custom_data"]);
                return ((CompoundTag)custom_dataTag["PublicBukkitValues"]);
            }
            else
            {
                return null;
            }
        }

        public string GetCustomName()
        {
            if (Components is not null && Components.ContainsKey("minecraft:custom_name"))
            {
                return ((StringTag)Components["minecraft:custom_name"]).Value;
            }
            else
            {
                return null;
            }
        }

        public CompoundTag GetEnchantments()
        {
            if (Components is not null && Components.ContainsKey("minecraft:enchantments"))
            {
                Texture = Name + "_enchanted";
                TextureUrl = "https://api.highgeek.eu/api/images/items/name/" + Texture;
                CompoundTag enchantmetsTag = ((CompoundTag)Components["minecraft:enchantments"]);
                return ((CompoundTag)enchantmetsTag["levels"]);
            }
            else
            {
                Texture = Name;
                TextureUrl = "https://api.highgeek.eu/api/images/items/name/" + Name;
                return null;
            }
        }

        public string GetItemString()
        {
            return CompoundTag.Stringify();
        }

        public static string WordsToUpper(string String)
        {
            String = String.Replace("_", " ");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            String = textInfo.ToTitleCase(String);
            return String;
        }
    }
}
