using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models.Redis
{
    public class RedisItem : RedisLivingObject
    {
        private GameItem _gameItem;
        public GameItem GameItem
        {
            get
            {
                return this._gameItem;
            }
            set
            {
                this._gameItem = value;
                //TODO: construct SNBT from item
                this._payload = value.CompoundTag.Stringify();
            }
        }

        public RedisItem() { }

        public RedisItem(string uuid, string payload, IRedisUpdateService redisUpdateService) : base(uuid, payload, redisUpdateService) 
        {
            this.GameItem = new GameItem(payload, uuid);
        }
        public RedisItem(string uuid, IRedisUpdateService redisUpdateService) : base(uuid, redisUpdateService)
        {
            this.GameItem = new GameItem(this.Payload, uuid);
        }

        public RedisItem(string uuid, IRedisUpdateService redisUpdateService, ILogger<RedisItemsService> logger) : base(uuid, redisUpdateService, logger)
        {
            _logger.LogInformation("Creating item: " + Payload);
            this.GameItem = new GameItem(this.Payload, uuid);
        }

        public override void OnRedisUpdate()
        {
            _logger.LogInformation("Updated item: " + Payload);
            this.GameItem = new GameItem(Payload, Uuid);
        }

        public override void OnRedisDelete()
        {
            Dispose();
        }
    }
}
