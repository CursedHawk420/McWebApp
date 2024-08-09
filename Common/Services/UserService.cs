using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using OpenApi.Highgeek.LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;
using Newtonsoft.Json;
using Highgeek.McWebApp.Common.Helpers.Channels;
using Microsoft.AspNetCore.Components.Authorization;
using Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter;
using Microsoft.Extensions.Logging;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IUserService : IDisposable
    {
        public ApplicationUser ApplicationUser { get; set; }

        public User LpUser { get; set; }

        public MinecraftUser MinecraftUser { get; set; }

        public bool HasConnectedAccount { get; set; }

        public bool Loaded { get; set; }

        public PlayerServerSettings PlayerServerSettings { get; set; }
        public ChannelSettingsAdapter ChannelOut { get; set; }
        public List<ChannelSettingsAdapter> JoinedChannels { get; set; }
        public List<ChannelSettingsAdapter> AvaiableChannels { get; set; }

        public Task UserServiceInitAsync(ApplicationUser applicationUser);
        public Task DisconnectGameAccount();
        public Task UpdatePlayerSettings();
    }
    public class UserService : IDisposable, IUserService
    {
        private readonly MinecraftUserManager _mcUserManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IRefreshService _refreshService;

        private readonly IRedisUpdateService _redisUpdateService;

        private readonly LuckPermsService _luckPermsService;

        private readonly ILocalizer _localizer;

        private readonly ICookieService _cookieService;

        private readonly ILogger<UserService> _logger;

        public ApplicationUser ApplicationUser { get; set; }

        public User LpUser { get; set; }

        public MinecraftUser MinecraftUser { get; set; }

        public bool HasConnectedAccount { get; set; }


        public bool Loaded { get; set; }

        public PlayerServerSettings PlayerServerSettings { get; set; }

        public ChannelSettingsAdapter ChannelOut { get; set; }
        public List<ChannelSettingsAdapter> JoinedChannels { get; set; }
        public List<ChannelSettingsAdapter> AvaiableChannels { get; set; }

        public UserService(MinecraftUserManager minecraftUserManager, UserManager<ApplicationUser> userManager, IRefreshService refreshService, LuckPermsService luckPermsService, IRedisUpdateService redisUpdateService, ILocalizer localizer, ICookieService cookieService, ILogger<UserService> logger)
        {
            Loaded = false;
            HasConnectedAccount = false;

            _mcUserManager = minecraftUserManager;
            _userManager = userManager;
            _refreshService = refreshService;
            _luckPermsService = luckPermsService;
            _redisUpdateService = redisUpdateService;
            _localizer = localizer;
            _cookieService = cookieService;
            _logger = logger;

            ChannelOut = new ChannelSettingsAdapter();
            JoinedChannels = new List<ChannelSettingsAdapter>();
            AvaiableChannels = new List<ChannelSettingsAdapter>();

            _refreshService.ServiceRefreshRequested += RefreshServiceState;
            _redisUpdateService.PlayersSettingsChanged += FetchPlayerSettingsFromRedis;
            _redisUpdateService.LuckpermsChanged += ListenForLuckUpdate;

        }
        
        public async Task UserServiceInitAsync(ApplicationUser applicationUser)
        {
            if(applicationUser is not null)
            {
                ApplicationUser = applicationUser;
                if (ApplicationUser is not null)
                {
                    if (ApplicationUser.mcUUID != null)
                    {
                        await SetMinecraftUserAsync(ApplicationUser.mcUUID);
                        _refreshService.CallInventoryServiceRefresh();
                    }
                    else
                    {
                        HasConnectedAccount = false;
                    }
                }
            }
            _localizer.Locale = await _cookieService.GetValue("appLocale");
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

        public async void ListenForLuckUpdate(object? sender, LuckpermsRedisLogAdapter redisLogAdapter)
        {
            if (ApplicationUser is not null && ApplicationUser.mcNickname is not null)
            {
                if (redisLogAdapter.TargetUuid.Equals(ApplicationUser.mcUUID))
                {
                    await SetLuckpermsUser(ApplicationUser.mcUUID);
                }
            }
        }

        public async Task SetLuckpermsUser(string uuid)
        {
            LpUser = await _luckPermsService.GetUserAsync(uuid);
        }

        public async void RefreshServiceState()
        {
            if(ApplicationUser is not null)
            {
                ApplicationUser = await _userManager.FindByIdAsync(ApplicationUser.Id);
                await UserServiceInitAsync(ApplicationUser);
            }
        }

        public async Task DisconnectGameAccount(){
            MinecraftUser = null;
            LpUser = null;
            HasConnectedAccount = false;
            ApplicationUser = await _userManager.FindByIdAsync(ApplicationUser.Id);
            _refreshService.CallInventoryServiceRefresh();
        }

        public async void FetchPlayerSettingsFromRedis(object? sender, string uuid)
        {
            if(ApplicationUser is not null && ApplicationUser.mcNickname is not null)
            {
                if (uuid.Contains(ApplicationUser.mcNickname))
                {
                    await SetPlayerSettings();
                }
            }
        }

        public async Task SetPlayerSettings()
        {
            JoinedChannels.Clear();
            try
            {
                if (ApplicationUser is not null)
                {
                    PlayerServerSettings = JsonConvert.DeserializeObject<PlayerServerSettings>(await RedisService.GetFromRedisAsync("players:settings:" + ApplicationUser.mcNickname));

                    ChannelOut = AvaiableChannels.FirstOrDefault(x => x.Name == PlayerServerSettings.channelOut);

                    foreach (var channel in PlayerServerSettings.joinedChannels)
                    {
                        JoinedChannels.Add(AvaiableChannels.FirstOrDefault(x => x.Name == channel));
                    }

                    _refreshService.CallChatServiceRefresh();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning("SetPlayerSettings() failed!: \nMessage: " + e.Message + "\nStacktrace: \n" + e.StackTrace);
            }
        }

        public async Task SetAvaiableChannels()
        {
            foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                AvaiableChannels.Add(ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedisAsync(key)));
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
