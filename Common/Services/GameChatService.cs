using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Models;
using OpenApi.Highgeek.LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;
using Highgeek.McWebApp.Common.Helpers.Channels;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Web;
using Org.BouncyCastle.Asn1.BC;
using Microsoft.AspNetCore.Components.Authorization;
using Sharpdactyl.Models.User;
using Highgeek.McWebApp.Common.Helpers;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IGameChatService
    {
        public List<RedisChatEntryAdapter> chat { get; set; }

        public Task<RedisChatEntryAdapter> CreateMessage(string message);

        public Task SendMessage(RedisChatEntryAdapter message);
    }
    public class GameChatService : IDisposable, IGameChatService
    {
        private readonly LuckPermsService _luckPermsService;
        private readonly IUserService _userService;
        private readonly IRefreshService _refreshService;
        private readonly IRedisUpdateService _redisUpdateService;

        public List<RedisChatEntryAdapter> chat { get; set; }

        public GameChatService(LuckPermsService luckPermsService, IUserService userService, IRedisUpdateService redisUpdateService, IRefreshService refreshService)
        {
            _luckPermsService = luckPermsService;
            _userService = userService;
            _redisUpdateService = redisUpdateService;
            _refreshService = refreshService;

            chat = new List<RedisChatEntryAdapter>();


            LoadChatFromRedis(_userService.PlayerServerSettings.joinedChannels);

            _redisUpdateService.ChatChanged += c_RenderNewChatEntry;
            _refreshService.ChatServiceRefreshRequested += RefreshChatService;
        }

        public async void RefreshChatService()
        {
            await Task.Run(async ()=> await LoadChatFromRedis(_userService.PlayerServerSettings.joinedChannels));
        }

        private async Task LoadChatFromRedis(List<string> channels)
        {
            try
            {
                chat.Clear();
                foreach (var channel in channels)
                {
                    var keys = await RedisService.GetKeysList("*chat:" + channel + "*");
                    keys = keys.OrderByDescending(i => i).ToList();
                    if (keys.Count > 0)
                    {
                        int max;
                        if (keys.Count < 50)
                        {
                            max = keys.Count;
                        }
                        else
                        {
                            max = 50;
                        }
                        for (int i = 0; i < max; i++)
                        {
                            chat.Add(RedisChatEntryAdapter.FromJson(await RedisService.GetFromRedisAsync(keys[i])));
                        }
                    }
                }
                chat = chat.OrderBy(i => i.Datetime).ToList();
                _refreshService.CallChatRefresh();
            }catch(Exception ex)
            {
                ex.WriteExceptionToRedis();
            }
        }

        private async void c_RenderNewChatEntry(object? sender, RedisChatEntryAdapter entry)
        {
            if (_userService.PlayerServerSettings.joinedChannels.Contains(entry.Channel))
            {
                chat.Add(entry);
                chat = chat.OrderBy(i => i.Datetime).ToList();
                _refreshService.CallChatRefresh();
            }
        }

        public async Task<RedisChatEntryAdapter> CreateMessage(string message)
        {
            ChannelSettingsAdapter channelOut = new ChannelSettingsAdapter();

            RedisChatEntryAdapter json = new RedisChatEntryAdapter();

            //ChannelSettingsAdapter channelSetting = ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedis("settings:server:chat:channels:"+channel));
            json.Channelprefix = _userService.ChannelOut.Prefix;
            json.Channel = _userService.ChannelOut.Name;
            json.PrettyServerName = "&2Web";

            json.Message = message;


            json.Prefix = _userService.LpUser.Metadata.Prefix;
            json.Suffix = _userService.LpUser.Metadata.Suffix;
            json.Primarygroup = _userService.LpUser.Metadata.PrimaryGroup;


            json.Username = _userService.MinecraftUser.NickName;
            json.Nickname = _userService.MinecraftUser.NickName;
            json.PlayerUuid = _userService.MinecraftUser.Uuid;


            DateTime dateTime = DateTime.UtcNow;

            string date = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF");

            json.Source = "web";
            json.Servername = "web";
            json.Datetime = dateTime;


            json.Uuid = "chat:" + json.Channel + ":" + date.Replace(":", "-") + ":" + json.Username;

            return json;
        }

        public async Task SendMessage(RedisChatEntryAdapter message)
        {
            if (message == null) { return;}

            await RedisService.SetInRedis(message.Uuid, message.ToJson());
        }


        private bool _disposed = false;

        void IDisposable.Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    // For example: Close file handles, database connections, etc.

                    _redisUpdateService.ChatChanged -= c_RenderNewChatEntry;
                    _refreshService.ChatServiceRefreshRequested -= RefreshChatService;
                }

                // Dispose unmanaged resources
                // For example: Release memory allocated through unmanaged code

                chat = null;
                _disposed = true;
            }
        }

        ~GameChatService()
        {
            Dispose(false); // Release unmanaged resources if the Dispose method wasn't called explicitly
        }

    }
}
