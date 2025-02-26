using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models.Redis
{
    public interface IRedisLivingObject
    {
        string Uuid { get; set; }
        string Payload { get; set; }
    }

    public class RedisLivingObject : IRedisLivingObject, IDisposable
    {
        public readonly IRedisUpdateService? _redisUpdateService;
        public readonly ILogger<RedisItemsService>? _logger;
        private bool disposedValue;

        protected string _uuid;
        public string Uuid
        {
            get
            {
                return this._uuid;
            }
            set
            {
                string oldUuid = this._uuid;
                _uuid = value;
                if (_redisUpdateService is not null)
                {
                    RedisService.DelFromRedis(oldUuid);
                    RedisService.SetInRedis(Uuid, Payload);
                }
            }
        }
        protected string _payload;
        public string Payload
        {
            get
            {
                return this._payload;
            }
            set
            {
                _payload = value;
                if(_redisUpdateService is not null)
                {
                    RedisService.SetInRedis(Uuid, value);
                }
            }
        }

        public RedisLivingObject(){ Uuid = ""; Payload = ""; }

        public RedisLivingObject(string uuid, string payload)
        {
            this.Payload = payload;
            this.Uuid = uuid;
        }

        public RedisLivingObject(string uuid, IRedisUpdateService redisUpdateService, ILogger<RedisItemsService> logger)
        {
            _logger = logger;
            this.Payload = RedisService.GetFromRedis(uuid);
            this.Uuid = uuid;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.KeyExpiredEvent += KeyExpiredEvent;
            _redisUpdateService.KeyDelEvent += KeyDelEvent;
            _redisUpdateService.KeySetEvent += KeySetEvent;
        }

        public RedisLivingObject(string uuid, IRedisUpdateService redisUpdateService)
        {
            this.Payload = RedisService.GetFromRedis(uuid);
            this.Uuid = uuid;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.KeyExpiredEvent += KeyExpiredEvent;
            _redisUpdateService.KeyDelEvent += KeyDelEvent;
            _redisUpdateService.KeySetEvent += KeySetEvent;
        }

        public RedisLivingObject(string uuid, string payload, IRedisUpdateService redisUpdateService)
        {
            this.Payload = payload;
            this.Uuid = uuid;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.KeyExpiredEvent += KeyExpiredEvent;
            _redisUpdateService.KeyDelEvent += KeyDelEvent;
            _redisUpdateService.KeySetEvent += KeySetEvent;
        }



        void KeySetEvent(object? sender, string uuid)
        {
            if(uuid == this.Uuid)
            {
                string newPayload = RedisService.GetFromRedis(uuid);
                _logger.LogInformation("Item: " + uuid);
                this._payload = newPayload;
                OnRedisUpdate();
            }
        }

        public virtual void OnRedisUpdate()
        {

        }

        void KeyExpiredEvent(object? sender, string uuid)
        {
            if (uuid == this.Uuid)
            {
                OnRedisDelete();
            }
        }

        void KeyDelEvent(object? sender, string uuid)
        {
            if(uuid == this.Uuid)
            {
                OnRedisDelete();
            }
        }

        public virtual void OnRedisDelete()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_redisUpdateService != null)
                    {
                        _redisUpdateService.KeyExpiredEvent -= KeyExpiredEvent;
                        _redisUpdateService.KeyDelEvent -= KeyDelEvent;
                        _redisUpdateService.KeySetEvent -= KeySetEvent;
                    }
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu.
                // TODO: Nastavte velká pole na hodnotu null.
                disposedValue = true;
            }
        }

        // TODO: Finalizační metodu přepište, jen pokud metoda Dispose(bool disposing) obsahuje kód pro uvolnění nespravovaných prostředků.
        ~RedisLivingObject()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
