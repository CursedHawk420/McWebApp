﻿using Highgeek.McWebApp.Common.Models.Adapters.Auction;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Services.Redis;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IAuctionService
    {
        public Task AddToAuction(string owner, long? price, GameItem gameItem);
        public IList<AuctionItem> Items { get; set; }

    }

    public class AuctionService : IAuctionService
    {
        private readonly IRedisUpdateService _redisUpdateService;

        private bool disposedValue;

        private readonly string keysprefix = "auction:";

        public IList<AuctionItem> Items { get; set; }

        public AuctionService(IRedisUpdateService redisUpdateService)
        {
            _redisUpdateService = redisUpdateService;
            Items = new List<AuctionItem>();

            _redisUpdateService.AuctionItemChange += AddToAuction;

            foreach (var uuid in RedisService.GetKeysList(keysprefix+"*").Result)
            {
                Items.Add(BuildAuctionItem(uuid));
            }
        }
        public async void AddToAuction(object? sender, string uuid)
        {
            var item = Items.FirstOrDefault(x => x.OriginUuid == uuid);
            if (item == null)
            {
                var it = BuildAuctionItem(uuid);
                Items.Add(it);
                _redisUpdateService.CallAuctionItemAddAction(it);
            }
        }

        public async Task AddToAuction(string owner, long? price, GameItem gameItem)
        {
            gameItem.ToAuctionItem(owner, price, _redisUpdateService);
        }

        public AuctionItem BuildAuctionItem(string uuid)
        {
            return new AuctionItem(uuid, RedisService.GetFromRedis(uuid), _redisUpdateService);
        }

    }
}
