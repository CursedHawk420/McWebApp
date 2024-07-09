using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SkinApi : Controller
    {
        private readonly MinecraftUserManager _minecraftUserManager;

        public SkinApi(MinecraftUserManager minecraftUserManager)
        {
            _minecraftUserManager = minecraftUserManager;
        }

        [AllowAnonymous]
        [Route("api/skins/playerhead/{playername}")]
        [HttpGet]
        public async Task<IActionResult> Get(string playername)
        {
            Byte[] b = await _minecraftUserManager.GetSkinHeadImage(playername);
            if (b == null)
            {
                b = await _minecraftUserManager.GetPremiumSkinHeadImage(playername);
                return File(b, "image/png");
            }
            return File(b, "image/png");
        }

        [AllowAnonymous]
        [Route("api/skins/playerheaduuid/{uuid}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayerHeadUuid(string uuid)
        {
            string playername = await _minecraftUserManager.GetPlayerNameFromUuid(uuid);
            Byte[] b = await _minecraftUserManager.GetSkinHeadImage(playername);
            if (b == null)
            {
                b = await _minecraftUserManager.GetPremiumSkinHeadImage(playername);
                return File(b, "image/png");
            }
            return File(b, "image/png");
        }

        [AllowAnonymous]
        [Route("api/skins/skintexture/{playername}")]
        [HttpGet]
        public async Task<IActionResult> GetSkintexture(string playername)
        {
            Byte[] b = await _minecraftUserManager.GetSkinTextureImage(playername);
            if (b == null)
            {
                b = await _minecraftUserManager.GetPremiumSkinTextureImage(playername);
                return File(b, "image/png");
            }
            return File(b, "image/png");
        }
    }
}
