using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Models.Redis;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Polly.Registry;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.FilterOperator;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public interface IRedisItemsService
    {
        public ConcurrentDictionary<string, IRedisLivingObject> Objects { get; }
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
        private readonly IAuctionService _auctionService;
        private readonly ILogger<RedisItemsService> _logger;
        private readonly IRefreshService _refreshService;
        private bool disposedValue;

        public string WinvIdentifier { get; set; }
        public ConcurrentDictionary<string, IRedisLivingObject> Objects { get; }
        public IList<IRedisLivingObject> AllItems
        {
            get
            {
                var allItems = new List<IRedisLivingObject>(Objects.Values.Count + _auctionService.Items.Count);
                allItems.AddRange(Objects.Values.ToList());
                allItems.AddRange(_auctionService.Items);
                return allItems.ToArray();
            }
        }
        public IList<VirtualInventory> VirtualInventories { get; set; }
        private ApplicationUser ApplicationUser { get; set; }

        public RedisItemsService(IRedisUpdateService redisUpdateService, IAuctionService auctionService, ILogger<RedisItemsService> logger, IRefreshService refreshService)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            _auctionService = auctionService;
            _refreshService = refreshService;
            Objects = new ConcurrentDictionary<string, IRedisLivingObject>();

            //_redisUpdateService.KeySetEvent += AddToDict;
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
                        try
                        {
                            Objects.TryAdd(uuid, new GameItem(uuid, item, _redisUpdateService));
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                foreach (var uuid in RedisService.GetKeysList("vinv:" + ApplicationUser.mcNickname + ":*").Result)
                {
                    string item = RedisService.GetFromRedis(uuid);
                    if (item != GameItem.AIRITEM)
                    {
                        try
                        {
                            Objects.TryAdd(uuid, new GameItem(uuid, item, _redisUpdateService));
                        }
                        catch(Exception e)
                        {

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
            if (info.rawuuid.StartsWith("winv:" + ApplicationUser.mcNickname) || info.rawuuid.StartsWith("vinv:" + ApplicationUser.mcNickname))
            {
                string item = RedisService.GetFromRedis(info.rawuuid);
                if (!Objects.ContainsKey(info.rawuuid))
                {
                    if (item != GameItem.AIRITEM)
                    {
                        try
                        {
                            Objects.TryAdd(info.rawuuid, new GameItem(info.rawuuid, item, _redisUpdateService));
                        }catch (Exception e)
                        {
                            _logger.LogInformation("Already in dictionary: " + info.rawuuid);
                        }
                    }
                }
                _refreshService.CallInventoryViewRefresh();
            }
        }
        /*
        void AddToDict(object? sender, string uuid)
        {
            if (uuid.StartsWith("winv:CursedHawk420:"))
            {
                if (!Objects.ContainsKey(uuid))
                {
                    Objects.Add(uuid, new GameItem(uuid, RedisService.GetFromRedis(uuid), _redisUpdateService, _logger));
                }
            }
        }*/

        void DeleteFromDict(object? sender, string uuid)
        {
            if (Objects.ContainsKey(uuid))
            {
                IRedisLivingObject removed;
                Objects.Remove(uuid, out removed);
                _refreshService.CallInventoryViewRefresh();
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
                    //_redisUpdateService.KeySetEvent -= AddToDict;
                    _redisUpdateService.InventoryChanged -= AddToDict;
                    _redisUpdateService.KeyDelEvent -= DeleteFromDict;
                    _redisUpdateService.KeyExpiredEvent -= DeleteFromDict;
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu.
                // TODO: Nastavte velká pole na hodnotu null.
                foreach(var obj in AllItems)
                {
                    obj.Dispose();
                }

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
