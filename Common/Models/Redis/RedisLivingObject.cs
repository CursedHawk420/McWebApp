﻿using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Services;
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
        public void Dispose();
    }

    public class RedisLivingObject : IRedisLivingObject, IDisposable
    {
        public readonly IRedisUpdateService? _redisUpdateService;
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

                    //RedisService.SetInRedis(oldUuid, GameItem.AIRITEM);
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

        public RedisLivingObject(){ _payload = ""; _uuid = ""; }

        public RedisLivingObject(string uuid, string payload)
        {
            this._payload = payload;
            this._uuid = uuid;
        }

        public RedisLivingObject(string uuid, IRedisUpdateService redisUpdateService)
        {
            this._payload = RedisService.GetFromRedis(uuid);
            this._uuid = uuid;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.KeyExpiredEvent += KeyExpiredEvent;
            _redisUpdateService.KeyDelEvent += KeyDelEvent;
            _redisUpdateService.KeySetEvent += KeySetEvent;
            _redisUpdateService.InventoryChanged += InventoryKeySetEvent;
        }

        public RedisLivingObject(string uuid, string payload, IRedisUpdateService redisUpdateService)
        {
            this._payload = payload;
            this._uuid = uuid;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.KeyExpiredEvent += KeyExpiredEvent;
            _redisUpdateService.KeyDelEvent += KeyDelEvent;
            _redisUpdateService.KeySetEvent += KeySetEvent;
            _redisUpdateService.InventoryChanged += InventoryKeySetEvent;
        }




        void InventoryKeySetEvent(object? sender, InventoryPositionInfo info)
        {
            if (info.rawuuid == this.Uuid)
            {
                this._payload = RedisService.GetFromRedis(info.rawuuid);
                OnRedisUpdate();
            }
        }

        void KeySetEvent(object? sender, string uuid)
        {
            if(uuid == this.Uuid)
            {
                this._payload = RedisService.GetFromRedis(uuid);
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
                        _redisUpdateService.InventoryChanged -= InventoryKeySetEvent;
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
