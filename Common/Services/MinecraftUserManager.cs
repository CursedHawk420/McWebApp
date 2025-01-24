using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Identity;
using Highgeek.McWebApp.Common.Models.mcserver_datadb;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharpdactyl.Models.User;
using System.Text;
using Highgeek.McWebApp.Common.Data;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Net;



namespace Highgeek.McWebApp.Common.Services
{
    public class MinecraftUserManager
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly McserverDatadbContext _mcDataDbContext;
        private readonly McserverEcodbContext _mcEcoDbContext;
        private readonly MineskinApiCommunication _mineskinApi;
        private readonly SkinManager _skinmanager;
        private readonly PteroManager _pteroManager;
        private readonly UsersDbContext _usersdb;
        private readonly ILogger<MinecraftUserManager> _logger;
        public MinecraftUserManager(
            UserManager<ApplicationUser> userManager,
            McserverMaindbContext mainDbContext,
            MineskinApiCommunication mineskinApiCommunication,
            SkinManager skinManager,
            McserverDatadbContext DataDbContext,
            McserverEcodbContext EcoDbContext,
            PteroManager pteroManager, 
            UsersDbContext usersdb, 
            ILogger<MinecraftUserManager> logger)
        {
            _userManager = userManager;
            _mcMainDbContext = mainDbContext;
            _mcDataDbContext = DataDbContext;
            _mcEcoDbContext = EcoDbContext;
            _mineskinApi = mineskinApiCommunication;
            _skinmanager = skinManager;
            _pteroManager = pteroManager;
            _usersdb = usersdb;
            _logger = logger;
        }

        public McserverMaindbContext GetMcserverMaindbContext()
        {
            return new McserverMaindbContext(); 
        }

        public async Task<MinecraftUser> GetUserAsync(string uuid)
        {
            return await GetMcserverMaindbContext().WebMinecraftusers.FirstOrDefaultAsync(s => s.Uuid == uuid);
        }
        public async Task<MinecraftUser> GetUserByNameAsync(string name)
        {
            return await GetMcserverMaindbContext().WebMinecraftusers.FirstOrDefaultAsync(s => s.NickName == name);
        }
        public static bool CheckPassword(string mcpassword, string mcusername, Authme authmeEntry)
        {
            return BCrypt.Net.BCrypt.Verify(mcpassword, authmeEntry.Password);
        }
        public async Task<StatusModel> RegisterMcUser(string mcusername, string password, ApplicationUser applicationUser)
        {
            var accountlinked = await _usersdb.Users.FirstOrDefaultAsync(s => s.mcNickname == mcusername);
            if (accountlinked != null)
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0001) Uživatel (" + applicationUser.Email+") se pokusil přidat již přiřazený herní účet ("+mcusername+")");
                return new StatusModel("minecraftusermanager-register-error-05", "(DB0001)");
            }
            var authmeEntry = await _mcMainDbContext.Authmes.FirstOrDefaultAsync(s => s.Realname == mcusername);
            if (authmeEntry == null)
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0201) Uživatel (" + applicationUser.Email+") se pokusil přidat neexistující herní účet v Authme ("+mcusername+")");
                return new StatusModel("minecraftusermanager-register-error-04", "(DB0201)");
            }
            var luckpermsPlayerEntry = await _mcMainDbContext.LuckpermsPlayers.FirstOrDefaultAsync(s => s.Username == mcusername.ToLower());
            if (luckpermsPlayerEntry == null)
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0101) Uživatel (" + applicationUser.Email + ") se pokusil přidat neexistující herní účet v LuckPermsPlayers (" + mcusername + ")");
                return new StatusModel("minecraftusermanager-register-error-03", "(DB0101)");
            }
            var premium = await _mcMainDbContext.Premia.FirstOrDefaultAsync(s => s.Name == mcusername);
            if (premium == null)
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0301) Uživatel (" + applicationUser.Email + ") se pokusil přidat neexistující herní účet v Premium (" + mcusername + ")");
                return new StatusModel("minecraftusermanager-register-error-02", "(DB0301)");
            }
            var minecraftaccountlinked = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.NickName == mcusername);
            /*if (minecraftaccountlinked != null && minecraftaccountlinked.ApplicationUserId != null)
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0401) Uživatel (" + applicationUser.Email + ") se pokusil přidat již přiřazený herní účet (" + mcusername + ")");
                return "Herní účet je již přiřazen k jinému účtu! (DB0401)";
            }*/
            if (CheckPassword(password, mcusername, authmeEntry))
            {
                MinecraftUser minecraftUser;

                if (minecraftaccountlinked == null)
                {
                    minecraftUser = new MinecraftUser();
                }
                else
                {
                    minecraftUser = minecraftaccountlinked;
                }

                minecraftUser.Uuid = luckpermsPlayerEntry.Uuid;
                minecraftUser.NickName = authmeEntry.Realname;
                minecraftUser.ApplicationUserId = applicationUser.Id;
                minecraftUser.ApplicationUserName = applicationUser.UserName;
                minecraftUser.ApplicationUserEmail = applicationUser.Email;
                minecraftUser.LpUserGroup = luckpermsPlayerEntry.PrimaryGroup;
                minecraftUser.EcoId = GetUserEcoId(luckpermsPlayerEntry.Uuid);

                if (premium.Premium1 == true)
                {
                    minecraftUser.IsPremium = true;
                    string uuid = ParseUUID(premium.Uuid.ToString());
                    minecraftUser.PremiumUuid = uuid;
                    _skinmanager.DefaultSkin(minecraftUser, applicationUser, uuid);
                }
                else
                {
                    minecraftUser.IsPremium = false;
                    _skinmanager.DefaultSkin(minecraftUser, applicationUser,null);
                }

                applicationUser.mcUUID = luckpermsPlayerEntry.Uuid;
                applicationUser.mcNickname = authmeEntry.Realname;
                await _userManager.UpdateAsync(applicationUser);

                if (minecraftaccountlinked == null)
                {
                    await _mcMainDbContext.AddAsync(minecraftUser);
                }

                await _mcMainDbContext.SaveChangesAsync();
                _logger.LogInformation("[MinecraftUserManager] Uživatel (" + applicationUser.Email + ") si přopojil herní účet (" + mcusername + ")");
                return new StatusModel("minecraftusermanager-register-succes-01");
            }
            else
            {
                _logger.LogWarning("[MinecraftUserManager] (DB0302) Uživatel (" + applicationUser.Email + ") zadal špatné heslo k hernímu účtu (" + mcusername + ")");
                return new StatusModel("minecraftusermanager-register-error-01", "wrong password (DB0302)");
            }
        }

        public string ParseUUID(string uuid)
        {
            uuid = uuid.Insert(8, "-");
            uuid = uuid.Insert(13, "-");
            uuid = uuid.Insert(18, "-");
            uuid = uuid.Insert(23, "-");
            return uuid.ToString();
        }

        public async Task ChangeIngamePassword()
        {

        }

        public async Task<StatusModel> DisconnectMinecraftAccount(ApplicationUser applicationUser)
        {
            if (applicationUser.mcNickname == null)
            {
                return new StatusModel("minecraftusermanager-unregister-error-01", "no account connected");
            }
            string mcusername = applicationUser.mcNickname;
            applicationUser.mcNickname = null;
            applicationUser.mcUUID = null;
            applicationUser.SkinHeadPicture = null;
            await _userManager.UpdateAsync(applicationUser);
            //await _userManager.RemoveFromRoleAsync(applicationUser, "MC");
            _logger.LogInformation("[MinecraftUserManager] Uživatel (" + applicationUser.Email + ") si odpojil herní účet (" + mcusername + ")");
            return new StatusModel("minecraftusermanager-unregister-succes-01");
        }

        public async Task<string> GetPlayerNameFromUuid(string uuid)
        {
            var result = await _mcMainDbContext.LuckpermsPlayers.FirstOrDefaultAsync(s => s.Uuid == uuid);
            if (result == null)
            {
                return "02a5451f-a4bf-3b07-89fa-595082556cc3";
            }
            else
            {
                return result.Username;
            }
        }

        public async Task<byte[]> GetSkinHeadImage(string playerName)
        {
            var minecraftUser = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.NickName == playerName);
            if (minecraftUser == null)
            {
                return null;
            }
            return minecraftUser.SkinHeadTextureImage;
        }

        public async Task<byte[]> GetPremiumSkinHeadImage(string playerName)
        {
            //Vytvořit image cache v db?
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://mc-heads.net/avatar/" + playerName))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    return imageBytes;
                }
            }
        }

        public async Task<byte[]> GetSkinTextureImage(string playerName)
        {
            var minecraftUser = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.NickName == playerName);
            if (minecraftUser == null)
            {
                return null;
            }
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(minecraftUser.SkinTexture))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    return imageBytes;
                }
            }
        }

        public async Task<byte[]> GetPremiumSkinTextureImage(string playerName)
        {
            //Vytvořit image cache v db?
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://mc-heads.net/skin/"+playerName))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    return imageBytes;
                }
            }
        }

        public async Task<StatusModel> SetPremiumAccount(string playerName)
        {
            MinecraftPremiumResponse minecraftPremiumResponse;
            using (WebClient wc = new WebClient())
            {
                Uri siteUri = new Uri("https://api.mojang.com/users/profiles/minecraft/" + playerName);
                try
                {
                    var response = wc.DownloadString(siteUri);
                    minecraftPremiumResponse = JsonConvert.DeserializeObject<MinecraftPremiumResponse>(response);
                    return await SetPremiumAccountinDatabase(playerName, minecraftPremiumResponse.id);

                }
                catch (WebException ex) when (ex.Response is HttpWebResponse wr && wr.StatusCode == HttpStatusCode.NotFound)
                {
                    return new StatusModel("minecraftusermanager-setpremium-error-01", false);
                }
            }
        }

        public async Task<StatusModel> SetPremiumAccountinDatabase(string playerName, string uuid)
        {
            var premium = await _mcMainDbContext.Premia.FirstOrDefaultAsync(s => s.Name == playerName);
            premium.Uuid = uuid;
            premium.Premium1 = true;
            await _mcMainDbContext.SaveChangesAsync();
            return new StatusModel("minecraftusermanager-setpremium-succes-01");
        }

        public async Task<StatusModel> UnsetPremiumAccount(string playerName)
        {
            var premium = await _mcMainDbContext.Premia.FirstOrDefaultAsync(s => s.Name == playerName);
            premium.Premium1 = false;
            premium.Uuid = null;
            await _mcMainDbContext.SaveChangesAsync();
            return new StatusModel("minecraftusermanager-unsetpremium-succes-01");
        }

        public static string UuidFuy(string uuid)
        {
            uuid = uuid.Remove(8, 1);
            uuid = uuid.Remove(12, 1);
            uuid = uuid.Remove(16, 1);
            uuid = uuid.Remove(20, 1);
            return uuid.ToString();
        }

        public static byte[] GetUserEcoId(string uuid)
        {
            uuid = UuidFuy(uuid);
            byte[] id = UUID.FromString(uuid);
            return id;

        }

        public async Task SetEcoIdForExistingUser(string uuid)
        {
            byte[] ecoid = GetUserEcoId(uuid);
            var minecraftuser = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.Uuid == uuid);
            minecraftuser.EcoId = ecoid;
            await _mcMainDbContext.SaveChangesAsync();
        }
    }
    public class MinecraftPremiumResponse
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
