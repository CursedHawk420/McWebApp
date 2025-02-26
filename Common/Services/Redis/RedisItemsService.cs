using Highgeek.McWebApp.Common.Models.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public interface IRedisItemsService
    {
        public IDictionary<string, IRedisLivingObject> Objects { get; }
    }
    //just testing
    public class RedisItemsService : IRedisItemsService
    {
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly ILogger<RedisItemsService> _logger;

        public IDictionary<string, IRedisLivingObject> Objects { get; }

        public RedisItemsService(IRedisUpdateService redisUpdateService, ILogger<RedisItemsService> logger)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            Objects = new Dictionary<string, IRedisLivingObject>();
            //TODO: GameItem inherit from RedisLivingObject. Better SET event calls. Load all items from redis to this dictionary, specify user! Connect to MainLayout instead of current InventoryService
            foreach (var item in RedisService.GetKeysList("winv:CursedHawk420:3f2bf628-2647-4c1e-a0f0-d21a6c71a7d3:*").Result)
            {
                Objects.Add(item, new RedisItem(item, _redisUpdateService, _logger));
            }
            _redisUpdateService.KeySetEvent += AddToDict;
            _redisUpdateService.KeyDelEvent += DeleteFromDict;
            _redisUpdateService.KeyExpiredEvent += DeleteFromDict;

        }

        void AddToDict(object? sender, string uuid)
        {
            if (uuid.StartsWith("winv:CursedHawk420:"))
            {
                if (!Objects.ContainsKey(uuid))
                {
                    Objects.Add(uuid, new RedisItem(uuid, _redisUpdateService, _logger));
                }
            }
        }

        void DeleteFromDict(object? sender, string uuid)
        {
            Objects.Remove(uuid);
        }
    }
}
