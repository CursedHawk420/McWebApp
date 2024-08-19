using Highgeek.McWebApp.Common.Models.Minecraft.ServerListModel;
using Highgeek.McWebApp.Common.Services.Redis;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IServerListService
    {
        event Action ServerListRefreshRequested;
        void CallServerListRefresh();

        List<ServerListModel> GetServerList();
    }

    public class ServerListService : IServerListService
    {
        private readonly IRedisUpdateService _redisUpdateService;

        public List<ServerListModel> ServerList { get; set; } = new List<ServerListModel>();


        public ServerListService(IRedisUpdateService redisUpdateService)
        {
            _redisUpdateService = redisUpdateService;

            _redisUpdateService.PlayersListChanged += RefreshServerList;
            Init();
        }

        public List<ServerListModel> GetServerList()
        {
            return this.ServerList.OrderBy(o => o.Position).ToList();
        }

        public async void Init()
        {

            foreach (var key in await RedisService.GetKeysList("server:*"))
            {
                ServerList.Add(ServerListModel.FromJson(await RedisService.GetFromRedisAsync(key)));
            }

            CallServerListRefresh();
        }

        public async void RefreshServerList(object sender, string uuid)
        {
            ServerListModel serverListModel = ServerListModel.FromJson(await RedisService.GetFromRedisAsync(uuid));
            ServerList.Remove(ServerList.FirstOrDefault(x => x.ServerName == serverListModel.ServerName));
            ServerList.Add(serverListModel);
            CallServerListRefresh();
        }



        public event Action ServerListRefreshRequested;
        public void CallServerListRefresh()
        {
            ServerListRefreshRequested?.Invoke();
        }
    }
}
