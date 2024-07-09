using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Identity;
using MineSkinApi.Model;
using Highgeek.McWebApp.Common.Data;
using Microsoft.EntityFrameworkCore;
using ServerTapApi.Model;
using System.Numerics;
using System.Net;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.Extensions.Logging;

namespace Highgeek.McWebApp.Common.Services
{
    /*public interface ISkinManager 
    {
        Task UpdateSkinInterfaceAsync(GenerateUrlPost200Response response, ApplicationUser user, string url);
    }*/
    public class SkinManager : IAsyncDisposable
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly MineskinApiCommunication _mineskinApi;
        private readonly ILogger<MinecraftUserManager> _logger;
        public SkinManager(UserManager<ApplicationUser> userManager, McserverMaindbContext mainDbContext, MineskinApiCommunication mineskinApiCommunication, ILogger<MinecraftUserManager> logger)
        {
            //zjistit jestli je potřeba usermanager
            _userManager = userManager;
            _mcMainDbContext = mainDbContext;
            _mineskinApi = mineskinApiCommunication;
            _logger = logger;
        }

        /*public Task UpdateSkinInterfaceAsync(GenerateUrlPost200Response response, ApplicationUser user, string url)
        {
            return UpdateSkinAsync(response, user, url);
        }*/

        public async Task<string> UpdateSkinAsync(GenerateUrlPost200Response response, ApplicationUser user, string url)
        {
            if (response != null)
            {
                //return response.ToString();
                var urlindex = await _mcMainDbContext.SrUrlIndices.FirstOrDefaultAsync(s => s.Url == url);
                if (urlindex == null)
                {
                    SrUrlIndex srurlindex = new SrUrlIndex();
                    srurlindex.Url = url;
                    srurlindex.SkinVariant = response.Variant.ToString().ToUpper();
                    await _mcMainDbContext.SrUrlIndices.AddAsync(srurlindex);
                }
                else
                {
                    urlindex.SkinVariant = response.Variant.ToString().ToUpper();
                }


                var urlskin = await _mcMainDbContext.SrUrlSkins.FirstOrDefaultAsync(s => s.Url == url);
                if (urlskin == null)
                {
                    SrUrlSkin srurlskin = new SrUrlSkin();
                    srurlskin.SkinVariant = response.Variant.ToString().ToUpper();
                    srurlskin.Signature = response.Data.Texture.Signature;
                    srurlskin.Value = response.Data.Texture.Value;
                    srurlskin.MineSkinId = response.Id.ToString();
                    srurlskin.Url = url;
                    await _mcMainDbContext.SrUrlSkins.AddAsync(srurlskin);
                }
                else
                {
                    urlskin.SkinVariant = response.Variant.ToString().ToUpper();
                    urlskin.Signature = response.Data.Texture.Signature;
                    urlskin.Value = response.Data.Texture.Value;
                    urlskin.MineSkinId = response.Id.ToString();
                }


                var skinuser = await _mcMainDbContext.SrPlayers.FirstOrDefaultAsync(s => s.Uuid == user.mcUUID);
                if (skinuser == null)
                {
                    SrPlayer player = new SrPlayer();
                    player.SkinVariant = response.Variant.ToString().ToUpper();
                    player.Uuid = user.mcUUID;
                    player.SkinIdentifier = url;
                    player.SkinType = "URL";
                    await _mcMainDbContext.SrPlayers.AddAsync(player);
                }
                else
                {
                    skinuser.SkinVariant = response.Variant.ToString().ToUpper();
                    skinuser.SkinIdentifier = url;
                    skinuser.SkinType = "URL";
                }
                await _mcMainDbContext.SaveChangesAsync();
                return "Skin nastaven úspěšně";

            }
            else
            {
                return "Chyba.";
            }
        }
        public async Task<string> SetHeadPicture(GenerateUrlPost200Response response, ApplicationUser applicationUser)
        {

            //Smazat prvních 38 znaků z url
            if (response == null)
            {
                return "Chyba v odkazu. Doporučujeme použít odkaz na skin z https://minecraft.novaskin.me/";
            }
            string headid = response.Data.Texture.Url.Remove(0, 38);
            string skinurl = "https://mc-heads.net/skin/" + headid;
            string headurl = "https://mc-heads.net/avatar/" + headid;
            var minecraftUser = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.Uuid == applicationUser.mcUUID);
            if (minecraftUser == null)
            {
                _logger.LogWarning("[SkinManager] (DB0401) Nebyl nalezen MinecraftUser uživatele (" + applicationUser.Email + ")");
                return "Uživatel nenalezen.";
            }
            await SaveSkinHeadImageToDatabase(minecraftUser, headurl);
            minecraftUser.SkinTexture = skinurl;
            minecraftUser.SkinHeadTexture = headid;
            await _mcMainDbContext.SaveChangesAsync();
            applicationUser.SkinHeadPicture = headid;
            await _userManager.UpdateAsync(applicationUser);
            return "Tvůj skin byl nastaven!";
        }

        public async Task DefaultSkin(MinecraftUser? minecraftUser, ApplicationUser applicationUser, string? uuid)
        {
            if (minecraftUser == null)
            {
                minecraftUser = await _mcMainDbContext.WebMinecraftusers.FirstOrDefaultAsync(s => s.Uuid == applicationUser.mcUUID);
                uuid = minecraftUser.PremiumUuid;
            }
            if (minecraftUser.IsPremium == true)
            {
                if (minecraftUser.SkinHeadTexture is not null)
                {
                    applicationUser.SkinHeadPicture = minecraftUser.SkinHeadTexture;
                }
                else
                {
                    applicationUser.SkinHeadPicture = uuid;
                }
                minecraftUser.SkinTexture = "https://mc-heads.net/skin/" + uuid;
                minecraftUser.SkinHeadTexture = uuid;
                string skinheadurl = "https://mc-heads.net/avatar/" + uuid;
                await SaveSkinHeadImageToDatabase(minecraftUser, skinheadurl);
                await _userManager.UpdateAsync(applicationUser);
            }
            else
            {
                if (minecraftUser.SkinHeadTexture is not null)
                {
                    applicationUser.SkinHeadPicture = minecraftUser.SkinHeadTexture;
                }
                else
                {
                    applicationUser.SkinHeadPicture = "b1e8cc7fb6c71f402f1269d83206896ec5b36592df7cc6101b1ec91963c4eb6f";
                }
                minecraftUser.SkinTexture = "http://textures.minecraft.net/texture/b1e8cc7fb6c71f402f1269d83206896ec5b36592df7cc6101b1ec91963c4eb6f";
                minecraftUser.SkinHeadTexture = "b1e8cc7fb6c71f402f1269d83206896ec5b36592df7cc6101b1ec91963c4eb6f";
                string skinheadurl = "https://mc-heads.net/avatar/" + "b1e8cc7fb6c71f402f1269d83206896ec5b36592df7cc6101b1ec91963c4eb6f";
                await SaveSkinHeadImageToDatabase(minecraftUser, skinheadurl);
                await _userManager.UpdateAsync(applicationUser);
            }
        }
        public async Task SaveSkinHeadImageToDatabase(MinecraftUser minecraftUser, string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    byte[] imageBytes =
                        await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    minecraftUser.SkinHeadTextureImage = imageBytes;
                    await _mcMainDbContext.SaveChangesAsync();
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
