﻿using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Highgeek.McWebApp.Common.Services
{
    public class InventoryService : IDisposable
    {
        private readonly ILogger<InventoryService> _logger;
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly UserService _userService;
        private readonly IRefreshService _refreshService;

        public InventoriesList InvData;

        public string WinvIdentifier;

        public int winvslots;

        public List<GameItem?> AllItems = new List<GameItem?>();

        public InventoryService(ILogger<InventoryService> logger, UserService userService, IRedisUpdateService redisUpdateService, IRefreshService refreshService)
        {
            _logger = logger;
            _userService = userService;
            _redisUpdateService = redisUpdateService;
            _refreshService = refreshService;
            //_iRedisUpdateService = _serviceProvider.GetService<IRedisUpdateService>();
            //_iRedisUpdateService.InventoryChanged += c_InventoryUpdated;
        }

        public async Task Init()
        {
            var context = new McserverMaindbContext();
            _logger.LogInformation("Loading player inventories...");
            InvData = new InventoriesList(context.VirtualInventories.Where(s => s.PlayerUuid == _userService.MinecraftUser.Uuid).ToList());

            foreach (var inv in InvData.Inventories)
            {
                string prefix = "";
                if (inv.Web)
                {
                    prefix = "winv";
                    WinvIdentifier = "winv:" + _userService.MinecraftUser.NickName + ":" + inv.InventoryUuid + ":";
                    winvslots = inv.Size;
                }
                else
                {
                    prefix = "vinv";
                }
                inv.Items = await LoadItemsFromRedis(inv, prefix);
                AllItems.AddRange(inv.Items);
            }
            _refreshService.CallInventoryRefresh();
        }




        public async Task<List<GameItem?>> LoadItemsFromRedis(VirtualInventory inventory, string prefix)
        {
            List<GameItem> items = new List<GameItem>();
            _logger.LogInformation("Starting loading data from redis for uuid: " + inventory.InventoryUuid);
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
            AllItems[int.Parse(info.position) + InvData.ListPosition[info.uuid]].Json = info.Item;
            AllItems[int.Parse(info.position) + InvData.ListPosition[info.uuid]] = new GameItem(info.Item, info.rawuuid);
        }

        public void Dispose()
        {
            AllItems.Clear();
            InvData.Inventories.Clear();
        }
    }
}
