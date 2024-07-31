using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Highgeek.McWebApp.Common.Services
{
    public class InventoryService
    {
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly ILogger<InventoryService> _logger;
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly UserService _userService;

        public InventoryService(McserverMaindbContext mcserverMaindb, ILogger<InventoryService> logger, UserService userService, IRedisUpdateService redisUpdateService)
        {
            _mcMainDbContext = mcserverMaindb;
            _logger = logger;
            _userService = userService;
            _redisUpdateService = redisUpdateService;
            //_iRedisUpdateService = _serviceProvider.GetService<IRedisUpdateService>();
            //_iRedisUpdateService.InventoryChanged += c_InventoryUpdated;
        }


        public string WinvIdentifier;
        public string VinvIdentifier;

        public int winvslots;
        public int vinvslots;

        public InventoryData wInvData = new InventoryData();
        public InventoryData vInvData = new InventoryData();

        public List<GameItem?> AllItems = new List<GameItem?>();


        public async Task<List<VirtualInventory>> GetInventoriesData()
        {
            _logger.LogInformation("Loading player inventories...");
            var context = new McserverMaindbContext();
            var toReturn = await context.VirtualInventories.Where(s => s.PlayerUuid == _userService.MinecraftUser.Uuid).ToListAsync();


            foreach ( var item in toReturn )
            {
                if (item.Web.Value)
                {
                    wInvData.VirtualInventory = item;
                    wInvData.Items = await LoadItemsFromRedis(item, "winv");
                }
                else
                {
                    vInvData.VirtualInventory = item;
                    vInvData.Items = await LoadItemsFromRedis(item, "vinv");
                }
            }

            AllItems.AddRange(vInvData.Items);
            AllItems.AddRange(wInvData.Items);

            WinvIdentifier = "winv:" + _userService.MinecraftUser.NickName + ":" + wInvData.VirtualInventory.InventoryUuid + ":";
            VinvIdentifier = "vinv:" + _userService.MinecraftUser.NickName + ":" + vInvData.VirtualInventory.InventoryUuid + ":";

            winvslots = wInvData.VirtualInventory.Size.Value;
            vinvslots = vInvData.VirtualInventory.Size.Value;

            return toReturn;
        }


        public async Task<List<GameItem?>> LoadItemsFromRedis(VirtualInventory inventory, string prefix)
        {

            _logger.LogInformation("Starting loading data from redis for uuid: " + inventory.InventoryUuid);
            List<GameItem> items = new List<GameItem>();
            for (int i = 0; i < inventory.Size; i++)
            {
                string uuid = prefix + ":" + _userService.MinecraftUser.NickName + ":" + inventory.InventoryUuid + ":" + i;
                string json = await RedisService.GetFromRedis(uuid);
                _logger.LogInformation("Loaded item from redis: " + json);
                GameItem item = new GameItem(json, uuid);
                _logger.LogInformation("ItemParser parsed item: " + item.Name);
                items.Add(item);
            }

            return items;
        }



        public async Task ItemPicked(MudDragAndDropItemTransaction<GameItem> pickItem)
        {
            //tooltip.Visible = false;
            //_logger.LogInformation("ItemParser.GetItemPositionInt(pickItem.SourceZoneIdentifier) = " + await ItemParser.GetItemPositionInt(pickItem.SourceZoneIdentifier));

        }
        public async Task ItemDroped(MudItemDropInfo<GameItem> dropItem)
        {
            string olduuid = dropItem.Item.Identifier;


            //dropItem.Item.Identifier = dropItem.DropzoneIdentifier;

            string item = await RedisService.GetFromRedis(olduuid);
            await RedisService.SetInRedis(olduuid, GameItem.AIRITEM);
            await RedisService.SetInRedis(dropItem.DropzoneIdentifier, item);
        }

        public async Task listUpdater(InventoryPositionInfo info)
        {
            _logger.LogInformation("Updating item: " + info.uuid + ":" + info.position);

            if (info.rawuuid.Contains("winv:"))
            {
                AllItems[int.Parse(info.position) + vInvData.VirtualInventory.Size.Value].Json = info.Item;
                AllItems[int.Parse(info.position) + vInvData.VirtualInventory.Size.Value] = new GameItem(info.Item, info.rawuuid);
            }
            else
            {
                AllItems[int.Parse(info.position)].Json = info.Item;
                AllItems[int.Parse(info.position)] = new GameItem(info.Item, info.rawuuid);
            }
        }
    }
}
