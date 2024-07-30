using Highgeek.McWebApp.Common.Models.Minecraft;
using Newtonsoft.Json;
using Minesharp.Nbt.Reader;
using SharpNBT;

namespace Highgeek.McWebApp.Common.Helpers
{
    public static class ItemParser
    {

        public static async Task<GameItem> CreateItem(string jsonString, int position, string originUuid)
        {
            GameItem gameItem = new GameItem();
            CompoundTag compoundTag = SharpNBT.SNBT.StringNbt.Parse(jsonString);
            gameItem.CompoundTag = compoundTag;

            string id = ((StringTag) compoundTag["id"]).Value;
            gameItem.Json = jsonString;


            int amount;
            try
            {
                amount = compoundTag.Get<IntTag>("count");
            }
            catch (Exception ex)
            {
                amount = 1;
                //Logger.LogError(ex.Message);
            }
            gameItem.Amount = amount;
            if (id != "minecraft:air")
            {
                gameItem.Identifier = originUuid;
            }

            //todo: check for enchants to add _enchanted.gif
            string texture = id.ToLower().Substring(id.IndexOf(":", id.Length));
            gameItem.Texture = texture;
            if (compoundTag.ContainsKey("minecraft:enchantments")){
                gameItem.TextureName = "https://api.highgeek.eu/api/images/items/name/" + texture + "_enchanted";
            }else{
                gameItem.TextureName = "https://api.highgeek.eu/api/images/items/name/" + texture;
            }


            /*
            var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);


            gameItem.Json = jsonString;

            string type = keyValuePairs["type"].ToString();
            string texture = keyValuePairs["type"].ToString().ToLower();
            gameItem.TextureName = texture;
            int amount;

            try
            {
                amount = int.Parse(keyValuePairs["amount"].ToString());
            }
            catch (Exception ex)
            {
                amount = 1;
                //Logger.LogError(ex.Message);
            }

            try
            {
                string meta = keyValuePairs["meta"].ToString();
                if (meta.Contains("enchants"))
                {
                    gameItem.TextureName = texture + "_enchanted";
                    texture = "https://inventories.chasem.dev/assets/minecraft/" + texture + "_enchanted.gif";
                    Dictionary<string, object> itemMeta = JsonConvert.DeserializeObject<Dictionary<string, object>>(meta);
                    itemMeta["enchants"].ToString();
                    gameItem.Enchantments = JsonConvert.DeserializeObject<Dictionary<string, int>>(itemMeta["enchants"].ToString());
                }
                else
                {
                    texture = "https://inventories.chasem.dev/assets/minecraft/" + texture + ".png";
                }
                if (meta.Contains("display-name"))
                {
                    DisplayNameParser(meta);
                }
                else
                {
                    gameItem.DisplayName = type;
                }
                


            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.Message);
                texture = "https://inventories.chasem.dev/assets/minecraft/" + texture + ".png";
            }

            //await _queue.StartProcessing(texture, texturename);

            //Task task = _imageCache.GetImageFromUrl(texture, texturename);
            gameItem.TextureName = "https://highgeek.eu/api/images/items/name/" + gameItem.TextureName;
            gameItem.Texture = texture;
            gameItem.name = type;
            gameItem.Amount = amount;
            //ImagesToCache.Add(texture, texturename);
            //gameItem.OriginUuid = originUuid;
            
            if (type != "AIR")
            {
                gameItem.Identifier = originUuid;

            }*/
            return gameItem;
        }

        public static async Task<string> rawUuidBuilder(string invuu, string mcusername, string prefix)
        {
            return prefix+":" + mcusername + ":" + invuu + ":";
        }

        public static string AIRITEM = "{\r\n    \"v\": 3465,\r\n    \"type\": \"AIR\"\r\n}";


        public static string DisplayNameParser(string name)
        {
            return name;
        }
    }
}
