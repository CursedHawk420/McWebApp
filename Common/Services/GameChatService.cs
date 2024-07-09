using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Models;
using LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;

namespace Highgeek.McWebApp.Common.Services
{
    public class GameChatService : IAsyncDisposable
    {
        private readonly LuckPermsService _luckPermsService;
        private readonly UserService _userService;

        public GameChatService(LuckPermsService luckPermsService, UserService userService) 
        {
            _luckPermsService = luckPermsService;
            _userService = userService;
            
        }

        //public MinecraftUser User { get; set; }

        public async Task<RedisChatEntryAdapter> CreateMessage(string message)
        {
            RedisChatEntryAdapter json = new RedisChatEntryAdapter();


            json.Message = message;


            json.Prefix = _userService.LpUser.Metadata.Prefix;
            json.Suffix = _userService.LpUser.Metadata.Suffix;
            json.Primarygroup = _userService.LpUser.Metadata.PrimaryGroup;

            json.Source = "web";
            json.Channelprefix = "&8[&2Global&8@&2Web&8]";
            json.Channel = "global";
            json.Servername = "web";


            json.Datetime = DateTime.Now.AddHours(2);
            json.Username = _userService.MinecraftUser.NickName;
            json.Nickname = _userService.MinecraftUser.NickName;
            json.PlayerUuid = _userService.MinecraftUser.Uuid;

            json.Uuid = "chat:"+ json.Channel + ":" + json.Datetime + ":" + _userService.MinecraftUser.NickName;
            return json;
        }

        public async Task SendMessage(RedisChatEntryAdapter message)
        {
            if (message == null) { return;}

            await RedisService.SetInRedis(message.Uuid, message.ToJson());
        }

        public ValueTask DisposeAsync()
        {
            //throw new NotImplementedException();
            return ValueTask.CompletedTask;
        }

    }
}
