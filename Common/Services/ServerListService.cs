using Highgeek.McWebApp.Common.Models.Minecraft.ServerListModel;
using Highgeek.McWebApp.Common.Services.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.FilterOperator;

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
            return this.ServerList;
        }

        public async void Init()
        {

            foreach (var key in await RedisService.GetKeysList("server:*"))
            {
                ServerList.Add(ServerListModel.FromJson(await RedisService.GetFromRedis(key)));
            }

            CallServerListRefresh();
        }

        public async void RefreshServerList(object sender, string uuid)
        {
            ServerListModel serverListModel = ServerListModel.FromJson(await RedisService.GetFromRedis(uuid));
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
