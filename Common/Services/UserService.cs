using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;
using Newtonsoft.Json;
using Highgeek.McWebApp.Common.Helpers.Channels;

namespace Highgeek.McWebApp.Common.Services
{
    public class UserService
    {
        private readonly MinecraftUserManager _mcUserManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IRefreshService _refreshService;

        private readonly IRedisUpdateService _redisUpdateService;

        private readonly LuckPermsService _luckPermsService;

        public ApplicationUser ApplicationUser;

        public MinecraftUser MinecraftUser { get; set; }

        public bool HasConnectedAccount = false;

        public User LpUser;

        public bool Loaded = false;

        public PlayerServerSettings PlayerServerSettings;

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
        }
        
        public async Task UserServiceInitAsync()
        {
            if (ApplicationUser.mcUUID != null)
            {
                await SetMinecraftUserAsync(ApplicationUser.mcUUID);
            }
            else
            {
                HasConnectedAccount = false;
            }
            Loaded = true;
            _refreshService.CallPageRefresh();
        }

        public async Task SetMinecraftUserAsync(string uuid)
        {
            MinecraftUser = await _mcUserManager.GetUserAsync(uuid);
            LpUser = await _luckPermsService.GetUserAsync(uuid);
            HasConnectedAccount = true;

            await SetAvaiableChannels();

            /*foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                AvaiableChannels.Add(ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedis(key)));
            }*/

            await SetPlayerSettings();
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
    }
}
