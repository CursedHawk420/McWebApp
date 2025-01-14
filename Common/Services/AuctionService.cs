using Highgeek.McWebApp.Common.Models.Adapters.Auction;
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

            foreach (var uuid in RedisService.GetKeysList(keysprefix+"*").Result)
            {
                Items.Add(BuildAuctionItem(uuid));
            }
        }

        public async Task AddToAuction(string owner, long? price, GameItem gameItem)
        {
            Items.Add(gameItem.ToAuctionItem(owner,price,_redisUpdateService));
        }

        public AuctionItem BuildAuctionItem(string uuid)
        {
            var adapter = AuctionItemAdapter.FromJson(RedisService.GetFromRedis(uuid));
            return new AuctionItem(_redisUpdateService, adapter.GameItem, uuid, adapter.Owner, adapter.Price, adapter.Datetime);
        }

    }
}
