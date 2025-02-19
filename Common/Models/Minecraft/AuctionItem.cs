using Highgeek.McWebApp.Common.Models.Adapters.Auction;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Services.Redis;
using NetTopologySuite.Densify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Highgeek.McWebApp.Common.Models.Minecraft
{
    public class AuctionItem : GameItem, IDisposable
    {
        public string? Owner { get; set; }
        public DateTime? TimeAdded { get; set; }
        public long? Price { get; set; }

        public IRedisUpdateService _redisUpdateService;

        private bool disposedValue;

        public AuctionItem(GameItem gameItem, string owner, long? price, IRedisUpdateService redisUpdateService)
        {
            InitInhertedProperties(gameItem);
            Owner = owner;
            Price = price;
            _redisUpdateService = redisUpdateService;

            var item = new AuctionItemAdapter();
            item.Owner = Owner;
            item.Price = Price;
            item.Datetime = DateTime.UtcNow.ToString();
            item.GameItem = Json;
            OriginUuid = Guid.NewGuid().ToString();
            Identifier = OriginUuid;

            RedisService.SetInRedis("auction:" + OriginUuid, item.ToJson());
        }
        public AuctionItem(IRedisUpdateService redisUpdateService)
        {
            _redisUpdateService = redisUpdateService;
        }

        public AuctionItem(IRedisUpdateService redisUpdateService, string json, string originUuid, string owner, long? price) : base(json, originUuid)
        {
            _redisUpdateService = redisUpdateService;

            _redisUpdateService.AuctionItemChange += ItemChanged;

            this.Price = price;
            this.Owner = owner;
            this.TimeAdded = DateTime.UtcNow;
        }

        public AuctionItem(IRedisUpdateService redisUpdateService, string json, string originUuid, string owner, long? price, string datetime) : base(json, originUuid)
        {
            _redisUpdateService = redisUpdateService;

            _redisUpdateService.AuctionItemChange += ItemChanged;

            this.Price = price;
            this.Owner = owner;
            if(DateTime.TryParse(datetime, out DateTime added))
            {
                this.TimeAdded = added;
            }
        }



        private async void ItemChanged(object? sender, string uuid)
        {
            try
            {
                if (uuid == OriginUuid)
                {
                    SetItem();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void SetItem()
        {

            var adapter = AuctionItemAdapter.FromJson(RedisService.GetFromRedis(OriginUuid));
            InitGameItem(adapter.GameItem, OriginUuid);
            Price = adapter.Price;
            Owner = adapter.Owner;
            TimeAdded = DateTime.Parse(adapter.Datetime);

            _redisUpdateService.CallAuctionItemChangeAction();
        }

        private void InitInhertedProperties(object baseClassInstance)
        {
            foreach (PropertyInfo propertyInfo in baseClassInstance.GetType().GetProperties())
            {
                object value = propertyInfo.GetValue(baseClassInstance, null);
                if (null != value) propertyInfo.SetValue(this, value, null);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _redisUpdateService.AuctionItemChange -= ItemChanged;
                }

                disposedValue = true;
            }
        }


        ~AuctionItem()
        {
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
