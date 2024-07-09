using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Highgeek.McWebApp.Api.Controllers
{
    public class DataApi : Controller
    {
        private readonly MinecraftUserManager _minecraftUserManager;
        private readonly ImageCacheService _imageCacheService;
        private readonly LuckPermsService _luckPermsService;

        public DataApi(MinecraftUserManager minecraftUserManager, ImageCacheService imageCacheService, LuckPermsService luckPermsService)
        {
            _minecraftUserManager = minecraftUserManager;
            _imageCacheService = imageCacheService;
            _luckPermsService = luckPermsService;

        }


        [AllowAnonymous]
        [HttpGet("api/data/getuuid/{name}")]
        public async Task<IActionResult> GetUuid(string name)
        {
            MinecraftUser user = await _minecraftUserManager.GetUserByNameAsync(name);
            return Json(user);
        }
    }
}
