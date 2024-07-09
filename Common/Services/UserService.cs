using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LuckPermsApi.Model;

namespace Highgeek.McWebApp.Common.Services
{
    public class UserService
    {
        private readonly MinecraftUserManager _mcUserManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IRefreshService _refreshService;

        private readonly LuckPermsService _luckPermsService;

        public ApplicationUser ApplicationUser;

        public MinecraftUser MinecraftUser;

        public bool HasConnectedAccount = false;

        public User LpUser;

        public bool Loaded = false;

        public UserService(MinecraftUserManager minecraftUserManager, UserManager<ApplicationUser> userManager, IRefreshService refreshService, LuckPermsService luckPermsService)
        {
            _mcUserManager = minecraftUserManager;
            _userManager = userManager;
            _refreshService = refreshService;
            _luckPermsService = luckPermsService;

            _refreshService.ServiceRefreshRequested += RefreshServiceState;
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

        public async Task<MinecraftUser> SetMinecraftUserAsync(string uuid)
        {
            MinecraftUser = await _mcUserManager.GetUserAsync(uuid);
            LpUser = await _luckPermsService.GetUserAsync(uuid);
            HasConnectedAccount = true;
            return MinecraftUser;
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
    }
}
