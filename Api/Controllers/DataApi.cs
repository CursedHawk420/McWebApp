using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Highgeek.McWebApp.Common.Models.Contexts;

namespace Highgeek.McWebApp.Api.Controllers
{
    public class DataApi : Controller
    {
        private readonly MinecraftUserManager _minecraftUserManager;
        private readonly ImageCacheService _imageCacheService;
        private readonly LuckPermsService _luckPermsService;
        private readonly McserverHusksyncContext _mcserverHusksyncContext;

        public DataApi(MinecraftUserManager minecraftUserManager, ImageCacheService imageCacheService, LuckPermsService luckPermsService, McserverHusksyncContext mcserverHusksyncContext)
        {
            _minecraftUserManager = minecraftUserManager;
            _imageCacheService = imageCacheService;
            _luckPermsService = luckPermsService;
            _mcserverHusksyncContext = mcserverHusksyncContext;

        }


        [AllowAnonymous]
        [HttpGet("api/data/getuuid/{name}")]
        public async Task<IActionResult> GetUuid(string name)
        {
            MinecraftUser user = await _minecraftUserManager.GetUserByNameAsync(name);
            return Json(user);
        }

        [AllowAnonymous]
        [HttpGet("api/data/money/uuid/{uuid}")]
        public async Task<IActionResult> GetXconomyFromUuid(string uuid)
        {
            var user = await _luckPermsService.GetXconomyFromUuid(uuid);
            return Json(user);
        }

        [AllowAnonymous]
        [HttpGet("api/data/money/name/{name}")]
        public async Task<IActionResult> GetXconomyFromName(string name)
        {
            var user = await _luckPermsService.GetXconomyFromName(name);
            return Json(user);
        }

        [AllowAnonymous]
        [HttpGet("api/data/husksync/name/{name}")]
        public async Task<IActionResult> GetHuskSyncFromName(string name)
        {
            var user = _mcserverHusksyncContext.HusksyncUsers.FirstOrDefault(x => x.Username == name);
            var result = _mcserverHusksyncContext.HusksyncUserData.FirstOrDefault(x => x.PlayerUu == user);
            return File(result.Data, "application/octet-stream");
        }
    }
}
