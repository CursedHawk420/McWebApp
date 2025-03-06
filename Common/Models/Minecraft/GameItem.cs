using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models.Adapters.Auction;
using Highgeek.McWebApp.Common.Models.Minecraft.DisplayName;
using Highgeek.McWebApp.Common.Models.Redis;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.Extensions.Logging;
using SharpNBT;
using SharpNBT.SNBT;
using System.Globalization;
using System.Net.Sockets;

namespace Highgeek.McWebApp.Common.Models.Minecraft
{
    public class GameItem : RedisLivingObject
    {
        public string? Texture { get; set; }
        public string? OriginUuid { get; set; }
        public CompoundTag? PublicBukkitValues { get; set; }
        public CompoundTag HeadPlayerProfile { get; set; }
        public CompoundTag HeadProperties { get; set; }
        public string PlayerHeadTextureId { get; set; }

        public virtual CompoundTag CompoundTag
        {
            get
            {
                try
                {
                    return StringNbt.Parse(Payload);
                }
                catch (Exception ex)
                {
                    return StringNbt.Parse("{\r\n    DataVersion: 3955,\r\n    count: 1,\r\n    id: \"minecraft:barrier\"\r\n}");
                }
            }
            set
            {
                if(CustomName != null)
                {
                    Payload = value.Stringify(true).Remove(0,1).Replace("\"minecraft:custom_name\":\"{\"extra\"", "\"minecraft:custom_name\":'{\"extra\"").Replace(",\"text\":\"\"}\",\"", ",\"text\":\"\"}',\"");
                }
                else
                {
                    Payload = value.Stringify(false);
                }
            }
        }

        public int Amount
        {
            get
            {
                try
                {
                    return CompoundTag.Get<IntTag>("count");
                }
                catch (Exception ex)
                {
                    return 1;
                }
            }
            set
            {
                CompoundTag newtag = CompoundTag;
                if (newtag.ContainsKey("count"))
                {
                    newtag.Remove("count");
                }
                newtag.Add(new IntTag("count", value));
                CompoundTag = newtag;
            }
        }

        public CompoundTag? Components
        {
            get
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
            set
            {
                CompoundTag newtag = CompoundTag;
                if (newtag.ContainsKey("components"))
                {
                    newtag.Remove("components");
                }
                newtag.Add(value);
                CompoundTag = newtag;
            }
        }

        public string? TextureUrl
        {
            get
            {
                if(Enchantments is not null &&  Enchantments.Count > 0)
                {
                    Texture = Name + "_enchanted";
                    return "https://api.highgeek.eu/api/images/items/name/" + Texture;
                }
                else
                {
                    Texture = Name;
                    return "https://api.highgeek.eu/api/images/items/name/" + Name;
                }
            }
        }

        public string? Id { 
            get{
                return ((StringTag)CompoundTag["id"]).Value;
            } 
            set{
                CompoundTag newtag = CompoundTag;
                ((StringTag)newtag["id"]).Value = value;
                CompoundTag = newtag;
            } 
        }

        public string? Name
        {
            get
            {
                return Id.ToLower().Substring(Id.IndexOf(":") + 1, Id.Length - Id.IndexOf(":") - 1);
            }
        }

        public string? CustomName
        {
            get
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
        }

        public DisplayNameAdapter? DisplayName
        {
            get
            {
                //todo: from compound tag get item display name
                if (CustomName != null)
                {
                    return DisplayNameAdapter.FromJson(CustomName);
                }
                else
                {
                    return new DisplayNameAdapter() { HtmlText =  WordsToUpper(Name) };
                }
            }
        }

        public CompoundTag? Enchantments
        {
            get
            {
                if (Components is not null && Components.ContainsKey("minecraft:enchantments"))
                {
                    return ((CompoundTag)((CompoundTag)Components["minecraft:enchantments"])["levels"]);
                }
                else
                {
                    return new CompoundTag("levels");
                }
            }
            set
            {
                if(value is not null)
                {
                    var newtag = new CompoundTag("components");
                    if (Components is not null)
                    {
                        newtag = Components;
                    }

                    if (!newtag.ContainsKey("minecraft:enchantments"))
                    {
                        newtag.Add(new CompoundTag("minecraft:enchantments"));
                        ((CompoundTag)newtag["minecraft:enchantments"]).Add(new CompoundTag("levels"));
                    }

                    ((CompoundTag)((CompoundTag)newtag["minecraft:enchantments"])["levels"]).Clear();
                    foreach (var tag in value)
                    {
                        ((CompoundTag)((CompoundTag)newtag["minecraft:enchantments"])["levels"]).Add(tag);
                    }
                    Components = newtag;
                }
            }
        }

        public ListTag? Modifiers
        {
            get
            {
                if (Components is not null && Components.ContainsKey("minecraft:attribute_modifiers"))
                {
                    return (ListTag)((CompoundTag)Components["minecraft:attribute_modifiers"])["modifiers"];
                }
                else
                {
                    return new ListTag("modifiers", TagType.Compound);
                }
            }
            set
            {
                if (value is not null)
                {
                    var newtag = new CompoundTag("components");
                    if (Components is not null)
                    {
                        newtag = Components;
                    }
                    if (!newtag.ContainsKey("minecraft:attribute_modifiers"))
                    {
                        newtag.Add(new CompoundTag("minecraft:attribute_modifiers"));
                        ((CompoundTag)newtag["minecraft:attribute_modifiers"]).Add(new ListTag("modifiers", TagType.Compound));
                    }

                    ((ListTag)((CompoundTag)newtag["minecraft:attribute_modifiers"])["modifiers"]).Clear();
                    foreach (var val in value)
                    {
                        ((ListTag)((CompoundTag)newtag["minecraft:attribute_modifiers"])["modifiers"]).Add(val);
                    }
                    Components = newtag;
                }
            }
        }



        public static string AIRITEM = "{\r\n    \"id\": \"minecraft:air\"\r\n}";

        public GameItem()
        {

        }

        public override void OnRedisDelete()
        {
            Dispose();
        }

        public override void OnRedisUpdate()
        {
            InitGameItem(Uuid);
        }

        //legacy mechanics

        public GameItem(string originUuid, string json)
        {
            _payload = json;
            _uuid = originUuid;
            InitGameItem(originUuid);
        }

        public GameItem(string originUuid, IRedisUpdateService updateService) : base(originUuid, updateService)
        {
            InitGameItem(originUuid);
        }

        public GameItem(string originUuid, string json, IRedisUpdateService updateService) : base(originUuid, json, updateService)
        {
            InitGameItem(originUuid);
        }

        public void InitGameItem(string originUuid)
        {
            this.OriginUuid = originUuid;

            if (Id != "minecraft:air")
            {
                _uuid = originUuid;
            }
            PublicBukkitValues = GetBukkitValues();

            if (Id == "minecraft:player_head")
            {
                try
                {
                    TrySetMinecraftProfile();
                }
                catch (Exception ex)
                {
                    ex.WriteExceptionToRedis();
                }
            }
        }

        public void TrySetMinecraftProfile(){
            if(Components.ContainsKey("minecraft:profile")){
                HeadPlayerProfile = (CompoundTag)Components["minecraft:profile"];
                if(HeadPlayerProfile.ContainsKey("properties")){
                    var list = (ListTag) HeadPlayerProfile["properties"];
                    HeadProperties = (CompoundTag) list[0];
                    if(HeadProperties.ContainsKey("value")){
                        var value = (StringTag) HeadProperties["value"];
                        byte[] data = Convert.FromBase64String(value);
                        string decodedString = System.Text.Encoding.UTF8.GetString(data);
                        int startIndex = decodedString.IndexOf("http://textures.minecraft.net/texture/") + 38;
                        int length = decodedString.Length - startIndex - 4;
                        PlayerHeadTextureId = decodedString.Substring(startIndex, length);
                    }
                }
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

    static class ExtensionMethods
    {
        public static AuctionItem ToAuctionItem(this GameItem source, string owner, long? price, IRedisUpdateService redisUpdateService)
        {
            AuctionItemAdapter AuctionItemAdapter = new AuctionItemAdapter();
            AuctionItemAdapter = new AuctionItemAdapter();
            AuctionItemAdapter.GameItem = source.Payload;
            AuctionItemAdapter.Price = price;
            AuctionItemAdapter.Owner = owner;
            AuctionItemAdapter.Datetime = DateTime.Now.ToString();
            string uuid = "auction:" + Guid.NewGuid().ToString();
            return new AuctionItem(uuid, AuctionItemAdapter, redisUpdateService);
        }
    }
}
