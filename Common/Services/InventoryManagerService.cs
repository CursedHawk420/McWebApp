﻿using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Highgeek.McWebApp.Common.Services.Redis;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;

namespace Highgeek.McWebApp.Common.Services
{
    public class InventoryManagerService : IAsyncDisposable, IDisposable
    {
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly ILogger<InventoryManagerService> _logger;
        private readonly ImageCacheService _imageCacheService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisUpdateService _iRedisUpdateService;
        public InventoryManagerService(McserverMaindbContext mcserverMaindb, ILogger<InventoryManagerService> logger, ImageCacheService imageCacheService, IServiceProvider serviceProvider)
        {
            _mcMainDbContext = mcserverMaindb;
            _logger = logger;
            _imageCacheService = imageCacheService;
            _serviceProvider = serviceProvider;
            //_iRedisUpdateService = _serviceProvider.GetService<IRedisUpdateService>();
            //_iRedisUpdateService.InventoryChanged += c_InventoryUpdated;
        }

        public string mcusername { get; set; }
        public string mcuuid { get; set; }
        public string invuuid { get; set; }
        public List<InventoryData> invData { get; set; } = new List<InventoryData>();


        /*public async Task<List<InventoryData>> GetInventoryAsync(string playeruuid)
        {
            List<Syncredisdatum> data = await GetInventoriesData(playeruuid);
            foreach (var datum in data)
            {

                _logger.LogInformation("Creating player inventory: " + datum.InventoryUuid);
                InventoryData inv = new InventoryData();
                inv.Syncredisdatum = datum;
                inv.Items = await LoadItemsFromRedis(datum.InventoryUuid);
                invData.Add(inv);
                _logger.LogInformation("Added player inventory: " + inv.Syncredisdatum.InventoryUuid);
            }
            return invData;
        }*/

        public async Task<List<VirtualInventory>> GetInventoriesData(string playeruuid)
        {
            _logger.LogInformation("Loading player inventories...");
            return await _mcMainDbContext.VirtualInventories.Where(s => s.PlayerUuid == playeruuid).ToListAsync();

        }

        //todo: load webinv data from somewhere - new table in db or just use redis keys?

        public async Task<List<GameItem?>> LoadItemsFromRedis(string id, string prefix)
        {

            _logger.LogInformation("Starting loading data from redis for uuid: " + id);
            List<GameItem> items = new List<GameItem>();
            try
            {
                for (int i = 0; i < 54; i++)
                {
                    string uuid = prefix + ":" + mcusername + ":" + id + ":" + i;
                    string json = await RedisService.GetFromRedis(uuid);
                    _logger.LogInformation("Loaded item from redis: " + json);
                    GameItem item = await ItemParser.CreateItem(json, i, uuid);
                    _logger.LogInformation("ItemParser parsed item: " + item.name);
                    items.Add(item);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("tryed to load bigger chest.: "+e.Message);
            }

            return items;
        }



        public void Dispose()
        {
            GC.Collect();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_mcMainDbContext).DisposeAsync();
        }
    }
}
