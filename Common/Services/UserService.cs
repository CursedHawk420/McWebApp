using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;
using Newtonsoft.Json;
using Highgeek.McWebApp.Common.Helpers.Channels;
using Microsoft.AspNetCore.Components.Authorization;
using Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter;

namespace Highgeek.McWebApp.Common.Services
{
    public class UserService : IDisposable
    {
        private readonly MinecraftUserManager _mcUserManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IRefreshService _refreshService;

        private readonly IRedisUpdateService _redisUpdateService;

        private readonly LuckPermsService _luckPermsService;

        public ApplicationUser ApplicationUser;

        public MinecraftUser MinecraftUser { get; set; }

        public bool HasConnectedAccount = false;

        public User LpUser { get; set; }

        public bool Loaded = false;

        public PlayerServerSettings PlayerServerSettings { get; set; }

        public ChannelSettingsAdapter ChannelOut;
        public List<ChannelSettingsAdapter> JoinedChannels = new List<ChannelSettingsAdapter>();
        public List<ChannelSettingsAdapter> AvaiableChannels = new List<ChannelSettingsAdapter>();

        public UserService(MinecraftUserManager minecraftUserManager, UserManager<ApplicationUser> userManager, IRefreshService refreshService, LuckPermsService luckPermsService, IRedisUpdateService redisUpdateService)
        {
            _mcUserManager = minecraftUserManager;
            _userManager = userManager;
            _refreshService = refreshService;
            _luckPermsService = luckPermsService;
            _redisUpdateService = redisUpdateService;


            _refreshService.ServiceRefreshRequested += RefreshServiceState;

            _redisUpdateService.PlayersSettingsChanged += FetchPlayerSettingsFromRedis;
            _redisUpdateService.LuckpermsChanged += ListenForLuckUpdate;
        }
        
        public async Task UserServiceInitAsync()
        {
            if(ApplicationUser is not null)
            {
                if (ApplicationUser.mcUUID != null)
                {
                    await SetMinecraftUserAsync(ApplicationUser.mcUUID);
                }
                else
                {
                    HasConnectedAccount = false;
                }
            }
            Loaded = true;
            _refreshService.CallPageRefresh();
        }

        public async Task SetMinecraftUserAsync(string uuid)
        {
            MinecraftUser = await _mcUserManager.GetUserAsync(uuid);
            await SetLuckpermsUser(uuid);
            HasConnectedAccount = true;

            await SetAvaiableChannels();

            /*foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                AvaiableChannels.Add(ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedis(key)));
            }*/

            await SetPlayerSettings();
        }

        public async void ListenForLuckUpdate(object sender, LuckpermsRedisLogAdapter redisLogAdapter)
        {

        }

        public async Task SetLuckpermsUser(string uuid)
        {
            LpUser = await _luckPermsService.GetUserAsync(uuid);
        }

        public async void RefreshServiceState()
        {
            await UserServiceInitAsync();
        }

        public async Task DisconnectGameAccount(){
            MinecraftUser = null;
            LpUser = null;
            HasConnectedAccount = false;
        }

        public async void FetchPlayerSettingsFromRedis(object sender, string uuid)
        {
            if (uuid.Contains(ApplicationUser.mcNickname))
            {
                await SetPlayerSettings();
            }
        }

        public async Task SetPlayerSettings()
        {
            JoinedChannels.Clear();
            PlayerServerSettings = JsonConvert.DeserializeObject<PlayerServerSettings>(await RedisService.GetFromRedis("players:settings:"+ApplicationUser.mcNickname));

            ChannelOut = AvaiableChannels.FirstOrDefault(x => x.Name == PlayerServerSettings.channelOut);

            foreach (var channel in PlayerServerSettings.joinedChannels)
            {
                JoinedChannels.Add(AvaiableChannels.FirstOrDefault(x => x.Name == channel));
            }

            _refreshService.CallChatServiceRefresh();
        }

        public async Task SetAvaiableChannels()
        {
            foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                AvaiableChannels.Add(ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedis(key)));
            }
        }

        public async Task UpdatePlayerSettings()
        {
            await RedisService.SetInRedis("players:settings:" + ApplicationUser.mcNickname, JsonConvert.SerializeObject(PlayerServerSettings));
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

                    _refreshService.ServiceRefreshRequested -= RefreshServiceState;
                    _redisUpdateService.PlayersSettingsChanged -= FetchPlayerSettingsFromRedis;
                    _redisUpdateService.LuckpermsChanged -= ListenForLuckUpdate;
                }

                // Dispose unmanaged resources
                // For example: Release memory allocated through unmanaged code

                this.ApplicationUser = null;
                this.MinecraftUser = null;
                this.LpUser = null;
                _disposed = true;
            }
        }

        ~UserService()
        {
            Dispose(false); // Release unmanaged resources if the Dispose method wasn't called explicitly
        }
    }
}
