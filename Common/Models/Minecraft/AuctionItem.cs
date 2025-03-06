using Highgeek.McWebApp.Common.Models.Adapters.Auction;
using Highgeek.McWebApp.Common.Models.Redis;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.Win32;
using NetTopologySuite.Densify;
using SharpNBT;
using SharpNBT.SNBT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Highgeek.McWebApp.Common.Models.Minecraft
{
    public interface IAuctionItem : INonMoveable
    {
        public string? Owner { get; set; }
        public DateTime? TimeAdded { get; set; }
        public long? Price { get; set; }
        public AuctionItemAdapter AuctionItemAdapter { get; set; }
    }

    public class AuctionItem : GameItem, IRedisLivingObject, IAuctionItem
    {
        public string? Owner { get; set; }
        public DateTime? TimeAdded { get; set; }
        public long? Price { get; set; }
        public AuctionItemAdapter AuctionItemAdapter { get; set; }
        private bool disposedValue;

        public override CompoundTag CompoundTag {
            get
            {
                try
                {
                    return StringNbt.Parse(AuctionItemAdapter.FromJson(Payload).GameItem);
                }
                catch (Exception ex)
                {
                    return StringNbt.Parse("{\r\n    DataVersion: 3955,\r\n    count: 1,\r\n    id: \"minecraft:barrier\"\r\n}");
                    //ex.WriteExceptionToRedis();
                }
            }
            set
            {
                if (CustomName != null)
                {
                    this.AuctionItemAdapter.GameItem = value.Stringify(true).Remove(0, 1).Replace("\"minecraft:custom_name\":\"{\"extra\"", "\"minecraft:custom_name\":'{\"extra\"").Replace(",\"text\":\"\"}\",\"", ",\"text\":\"\"}',\"");
                    Payload = this.AuctionItemAdapter.ToJson();
                }
                else
                {
                    this.AuctionItemAdapter.GameItem = value.Stringify(false);
                    Payload = this.AuctionItemAdapter.ToJson();
                }
            }
        }

        public AuctionItem() { }

        //existing AuctionItem
        public AuctionItem(string uuid, string json, IRedisUpdateService redisUpdateService) : base(uuid, json, redisUpdateService)
        {
            var obj = AuctionItemAdapter.FromJson(json);
            this.AuctionItemAdapter = obj;
            this.Owner = obj.Owner;
            this.Price = obj.Price;
            if(DateTime.TryParse(obj.Datetime, out DateTime result))
            {
                this.TimeAdded = result;
            }
            _redisUpdateService.AuctionItemChange += UpdateItem;
        }

        //new AuctionItem
        public AuctionItem(string uuid, AuctionItemAdapter auctionItemAdapter, IRedisUpdateService redisUpdateService) : base(uuid, auctionItemAdapter.ToJson(), redisUpdateService)
        {
            this.TimeAdded = DateTime.UtcNow;
            this.Owner = auctionItemAdapter.Owner;
            this.Price = auctionItemAdapter.Price;
            this.AuctionItemAdapter = auctionItemAdapter;
            this.Uuid = uuid;
        }

        void UpdateItem(object? sender, string uuid)
        {
            if(uuid == Uuid)
            {
                base.InitGameItem(uuid);
                _payload = RedisService.GetFromRedis(uuid);
                _redisUpdateService.CallAuctionItemChangeAction();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    _redisUpdateService.AuctionItemChange -= UpdateItem;
                }
            }
        }

        private void InitInhertedProperties(object baseClassInstance)
        {
            foreach (PropertyInfo propertyInfo in baseClassInstance.GetType().GetProperties())
            {
                object value = propertyInfo.GetValue(baseClassInstance, null);
                if (null != value) propertyInfo.SetValue(this, value, null);
            }
        }
    }
}
