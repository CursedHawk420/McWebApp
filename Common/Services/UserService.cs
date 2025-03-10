﻿using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using OpenApi.Highgeek.LuckPermsApi.Model;
using Highgeek.McWebApp.Common.Services.Redis;
using Newtonsoft.Json;
using Highgeek.McWebApp.Common.Helpers.Channels;
using Microsoft.AspNetCore.Components.Authorization;
using Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter;
using Highgeek.McWebApp.Common.Helpers;
using Microsoft.Extensions.Logging;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Microsoft.AspNetCore.Components;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IUserService : IDisposable
    {
        public ApplicationUser? ApplicationUser { get; set; }

        public User? LpUser { get; set; }

        public MinecraftUser? MinecraftUser { get; set; }

        public bool HasConnectedAccount { get; set; }

        public IRefreshService _refreshService { get; }

        public bool Loaded { get; set; }

        public string ServiceId { get; set; }

        public bool IsAdmin { get; set; }

        public PlayerServerSettings PlayerServerSettings { get; set; }
        public ChannelSettingsAdapter ChannelOut { get; set; }
        public List<ChannelSettingsAdapter> JoinedChannels { get; set; }
        public List<ChannelSettingsAdapter> AvaiableChannels { get; set; }

        public Task UserServiceInitAsync(ApplicationUser applicationUser);
        public Task DisconnectGameAccount();
        public Task UpdatePlayerSettings();

        //public Task<Dictionary<string, float>> GetEconomyModel();
        public Dictionary<string, float> Economy { get; set; }
        public Task<StatusModel> SetPremiumAccount();
        public Task<StatusModel> UnsetPremiumAccount();

        public bool HasPermission(string perm);

    }
    public class UserService : IDisposable, IUserService
    {
        private readonly MinecraftUserManager _mcUserManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public IRefreshService _refreshService { get; }

        private readonly IRedisUpdateService _redisUpdateService;

        private readonly LuckPermsService _luckPermsService;

        private readonly ILocalizer _localizer;

        private readonly ICookieService _cookieService;

        private readonly ILogger<UserService> _logger;

        private readonly NavigationManager _navigationManager;

        private readonly IRedisItemsService _redisItemsService;

        public ApplicationUser? ApplicationUser { get; set; }

        public User? LpUser { get; set; }

        public MinecraftUser? MinecraftUser { get; set; }

        public bool HasConnectedAccount { get; set; }

        public string ServiceId { get; set; } = Guid.NewGuid().ToString();

        public bool Loaded { get; set; }

        public bool IsAdmin { get; set; }

        public PlayerServerSettings? PlayerServerSettings { get; set; }

        public ChannelSettingsAdapter ChannelOut { get; set; }
        public List<ChannelSettingsAdapter> JoinedChannels { get; set; }
        public List<ChannelSettingsAdapter> AvaiableChannels { get; set; }

        public Dictionary<string, float> Economy { get; set; }

        public UserService(
            MinecraftUserManager minecraftUserManager, 
            UserManager<ApplicationUser> userManager, 
            IRefreshService refreshService, 
            LuckPermsService luckPermsService, 
            IRedisUpdateService redisUpdateService, 
            ILocalizer localizer, 
            ICookieService cookieService, 
            ILogger<UserService> logger, 
            NavigationManager navigationManager,
            IRedisItemsService redisItemsService
            )
        {
            Loaded = false;
            HasConnectedAccount = false;
            IsAdmin = false;

            _mcUserManager = minecraftUserManager;
            _userManager = userManager;
            _refreshService = refreshService;
            _luckPermsService = luckPermsService;
            _redisUpdateService = redisUpdateService;
            _localizer = localizer;
            _cookieService = cookieService;
            _logger = logger;
            _navigationManager = navigationManager;
            _redisItemsService = redisItemsService;

            ChannelOut = new ChannelSettingsAdapter();
            JoinedChannels = new List<ChannelSettingsAdapter>();
            AvaiableChannels = new List<ChannelSettingsAdapter>();
            Economy = new Dictionary<string, float>();

            _refreshService.ServiceRefreshRequested += RefreshServiceState;
            _redisUpdateService.PlayersSettingsChanged += FetchPlayerSettingsFromRedis;
            _redisUpdateService.LuckpermsChanged += ListenForLuckUpdate;
            _redisUpdateService.PlayersEconomyChanged += HandleEconomyChange;

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
            ChannelOut = new ChannelSettingsAdapter();
            JoinedChannels = new List<ChannelSettingsAdapter>();
            AvaiableChannels = new List<ChannelSettingsAdapter>();
            Economy = new Dictionary<string, float>();

            MinecraftUser = await _mcUserManager.GetUserAsync(uuid);
            await SetLuckpermsUser(uuid);
            await EconomyLoad();
            HasConnectedAccount = true;

            await SetAvaiableChannels();

            /*foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                AvaiableChannels.Add(ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedis(key)));
            }*/

            await SetPlayerSettings();
            await _redisItemsService.Init(ApplicationUser);
        }

        public async void ListenForLuckUpdate(object? sender, string uuid)
        {
            if (ApplicationUser is not null && ApplicationUser.mcNickname is not null)
            {
                var redisLogAdapter = LuckpermsRedisLogAdapter.FromJson(await RedisService.GetFromRedisAsync(uuid));
                if (redisLogAdapter.TargetUuid.Equals(ApplicationUser.mcUUID))
                {
                    await SetLuckpermsUser(ApplicationUser.mcUUID);
                }
            }
        }

        public async Task SetLuckpermsUser(string uuid)
        {
            LpUser = await _luckPermsService.GetUserAsync(uuid);

            if(LpUser is not null)
            {
                if (LpUser.ParentGroups.Contains("sa"))
                {
                    IsAdmin = true;
                }
            }
        }

        public async void RefreshServiceState()
        {
            if(ApplicationUser is not null)
            {
                ApplicationUser = await _userManager.FindByIdAsync(ApplicationUser.Id);
                await UserServiceInitAsync(ApplicationUser);
            }
        }

        public async Task DisconnectGameAccount()
        {
            ApplicationUser = await _userManager.FindByIdAsync(ApplicationUser.Id);
            ApplicationUser.mcNickname = null;
            ApplicationUser.mcUUID = null;
            MinecraftUser = null;
            LpUser = null;
            HasConnectedAccount = false;
            PlayerServerSettings = null;
            JoinedChannels = null;
            ChannelOut = null;
            AvaiableChannels = null;
            IsAdmin = false;
            Economy.Clear();
            _refreshService.CallInventoryServiceRefresh();
            _navigationManager.NavigateTo("/");
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
            catch (Exception ex)
            {
                ex.WriteExceptionToRedis();
                _logger.LogWarning("SetPlayerSettings() failed!: \nMessage: " + ex.Message + "\nStacktrace: \n" + ex.StackTrace);
            }
        }

        public async Task SetAvaiableChannels()
        {
            foreach (var key in await RedisService.GetKeysList("settings:server:chat:channels:*"))
            {
                var channel = ChannelSettingsAdapter.FromJson(await RedisService.GetFromRedisAsync(key));
                if(channel.SpeakPermission is not null)
                {
                    if (HasPermission(channel.SpeakPermission))
                    {
                        channel.CanSpeak = true;
                    }else
                    {
                        channel.CanSpeak = false;
                    }
                }
                else
                {
                    channel.CanSpeak = true;
                }
                if (channel.Permission is null)
                {
                    AvaiableChannels.Add(channel);
                }
                else
                {
                    if(HasPermission(channel.Permission))
                    {
                        AvaiableChannels.Add(channel);
                    }
                }
            }
        }

        public async Task UpdatePlayerSettings()
        {
            await RedisService.SetInRedis("players:settings:" + ApplicationUser.mcNickname, JsonConvert.SerializeObject(PlayerServerSettings));
        }


        public async Task EconomyLoad()
        {
            foreach (var uuid in await RedisService.GetKeysList("economy:players:" + ApplicationUser.mcNickname + ":*"))
            {
                if (float.TryParse(await RedisService.GetFromRedisAsync(uuid), out float inte))
                {
                    string id = uuid.Substring(uuid.LastIndexOf(":") + 1, uuid.Length - uuid.LastIndexOf(":") - 1);
                    Economy.Add(id, inte);
                }
            }
            _refreshService.CallEcoRefresh();
        }

        /*public async Task<Dictionary<string, float>> GetEconomyModel()
        {
            if(Economy is not null){
            return Economy;
            }else{
                await EconomyLoad();
                return await GetEconomyModel();
            }
        }*/
        public async void HandleEconomyChange(object? sender, string uuid)
        {
            if (HasConnectedAccount)
            {
                if (ApplicationUser.mcNickname is not null)
                {
                    if (uuid.Contains(ApplicationUser.mcNickname))
                    {
                        string id = uuid.Substring(uuid.LastIndexOf(":") + 1, uuid.Length - uuid.LastIndexOf(":") - 1);
                        if (Economy.ContainsKey(id))
                        {
                            Economy.Remove(id);
                        }
                        try
                        {
                            Economy.Add(id, float.Parse(await RedisService.GetFromRedisAsync(uuid)));
                        }
                        catch (Exception ex)
                        {
                            ex.WriteExceptionToRedis();
                        }
                        _refreshService.CallEcoRefresh();
                    }
                }
            }
        }

        public async Task<StatusModel> SetPremiumAccount()
        {
            return await _mcUserManager.SetPremiumAccount(MinecraftUser.NickName);
        }


        public async Task<StatusModel> UnsetPremiumAccount()
        {
            return await _mcUserManager.UnsetPremiumAccount(MinecraftUser.NickName);
        }

        public bool HasPermission(string perm)
        {
            return _luckPermsService.HasPermissionAsync(perm, MinecraftUser.Uuid).Result;
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
                    _redisUpdateService.PlayersEconomyChanged -= HandleEconomyChange;
                }

                // Dispose unmanaged resources
                // For example: Release memory allocated through unmanaged code

                this.ApplicationUser = null;
                this.MinecraftUser = null;
                this.LpUser = null;
                this.Economy = null;
                _disposed = true;
            }
        }

        ~UserService()
        {
            Dispose(false); // Release unmanaged resources if the Dispose method wasn't called explicitly
        }
    }
}
