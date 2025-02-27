using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Models.Redis;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.FilterOperator;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public interface IRedisItemsService
    {
        public IDictionary<string, IRedisLivingObject> Objects { get; }
        public IList<IRedisLivingObject> AllItems { get; }
        public Task Init(ApplicationUser applicationUser);
        public Task ItemPicked(MudDragAndDropItemTransaction<IRedisLivingObject> pickItem);
        public Task ItemDroped(MudItemDropInfo<IRedisLivingObject> dropItem);
        public string WinvIdentifier { get; set; }
        public IList<VirtualInventory> VirtualInventories { get; set; }

    }
    //just testing
    public class RedisItemsService : IRedisItemsService, IDisposable
    {
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly ILogger<RedisItemsService> _logger;
        private bool disposedValue;

        public string WinvIdentifier { get; set; }
        public IDictionary<string, IRedisLivingObject> Objects { get; }
        public IList<IRedisLivingObject> AllItems
        {
            get
            {
                return Objects.Values.ToList();
            }
        }
        public IList<VirtualInventory> VirtualInventories { get; set; }
        private ApplicationUser ApplicationUser { get; set; }

        public RedisItemsService(IRedisUpdateService redisUpdateService, ILogger<RedisItemsService> logger)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            Objects = new Dictionary<string, IRedisLivingObject>();

            _redisUpdateService.KeySetEvent += AddToDict;
            _redisUpdateService.InventoryChanged += AddToDict;
            _redisUpdateService.KeyDelEvent += DeleteFromDict;
            _redisUpdateService.KeyExpiredEvent += DeleteFromDict;
        }

        public async Task Init(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
            await GetPlayerInvsFromDb();
            await PopulateDictionary();
            //TODO: Better SET event calls. Connect to MainLayout instead of current InventoryService
        }

        public async Task PopulateDictionary()
        {
            foreach (var inv in VirtualInventories)
            {
                foreach (var uuid in RedisService.GetKeysList("winv:" + ApplicationUser.mcNickname + ":*").Result)
                {
                    WinvIdentifier = "winv:" + ApplicationUser.mcNickname + ":" + inv.InventoryUuid + ":";
                    string item = RedisService.GetFromRedis(uuid);
                    if (item != GameItem.AIRITEM)
                    {
                        if (!Objects.TryAdd(uuid, new GameItem(uuid, item, _redisUpdateService, _logger)))
                        {
                            Objects[uuid] = new GameItem(uuid, item, _redisUpdateService, _logger);
                        }
                    }
                }

                foreach (var uuid in RedisService.GetKeysList("vinv:" + ApplicationUser.mcNickname + ":*").Result)
                {
                    string item = RedisService.GetFromRedis(uuid);
                    if (item != GameItem.AIRITEM)
                    {
                        if (!Objects.TryAdd(uuid, new GameItem(uuid, item, _redisUpdateService, _logger)))
                        {
                            Objects[uuid] = new GameItem(uuid, item, _redisUpdateService, _logger);
                        }
                    }
                }
            }
        }

        public async Task GetPlayerInvsFromDb()
        {
            var context = new McserverMaindbContext();
            VirtualInventories = context.VirtualInventories.Where(s => s.PlayerUuid == ApplicationUser.mcUUID).ToList();
        }

        void AddToDict(object? sender, InventoryPositionInfo info)
        {
            if (info.rawuuid.StartsWith("winv:" + ApplicationUser.mcNickname))
            {
                if (!Objects.ContainsKey(info.rawuuid))
                {
                    string item = RedisService.GetFromRedis(info.rawuuid);
                    if (item != GameItem.AIRITEM)
                    {
                        if (!Objects.TryAdd(info.rawuuid, new GameItem(info.rawuuid, item, _redisUpdateService, _logger)))
                        {
                            Objects[info.rawuuid] = new GameItem(info.rawuuid, item, _redisUpdateService, _logger);
                        }
                    }
                }
            }
            if (info.rawuuid.StartsWith("vinv:" + ApplicationUser.mcNickname))
            {
                if (!Objects.ContainsKey(info.rawuuid))
                {
                    string item = RedisService.GetFromRedis(info.rawuuid);
                    if (item != GameItem.AIRITEM)
                    {
                        if (!Objects.TryAdd(info.rawuuid, new GameItem(info.rawuuid, item, _redisUpdateService, _logger)))
                        {
                            Objects[info.rawuuid] = new GameItem(info.rawuuid, item, _redisUpdateService, _logger);
                        }
                    }
                }
            }
        }

        void AddToDict(object? sender, string uuid)
        {
            if (uuid.StartsWith("winv:CursedHawk420:"))
            {
                if (!Objects.ContainsKey(uuid))
                {
                    Objects.Add(uuid, new GameItem(uuid, RedisService.GetFromRedis(uuid), _redisUpdateService, _logger));
                }
            }
        }

        void DeleteFromDict(object? sender, string uuid)
        {
            if (Objects.ContainsKey(uuid))
            {
                Objects.Remove(uuid);
            }
        }

        public async Task ItemPicked(MudDragAndDropItemTransaction<IRedisLivingObject> pickItem)
        {
            //tooltip.Visible = false;
            //_logger.LogInformation("ItemParser.GetItemPositionInt(pickItem.SourceZoneIdentifier) = " + await ItemParser.GetItemPositionInt(pickItem.SourceZoneIdentifier));

        }
        public async Task ItemDroped(MudItemDropInfo<IRedisLivingObject> dropItem)
        {
            dropItem.Item.Uuid = dropItem.DropzoneIdentifier;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _redisUpdateService.KeySetEvent -= AddToDict;
                    _redisUpdateService.InventoryChanged -= AddToDict;
                    _redisUpdateService.KeyDelEvent -= DeleteFromDict;
                    _redisUpdateService.KeyExpiredEvent -= DeleteFromDict;
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu.
                // TODO: Nastavte velká pole na hodnotu null.
                disposedValue = true;
            }
        }

        // // TODO: Finalizační metodu přepište, jen pokud metoda Dispose(bool disposing) obsahuje kód pro uvolnění nespravovaných prostředků.
        // ~RedisItemsService()
        // {
        //     // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
