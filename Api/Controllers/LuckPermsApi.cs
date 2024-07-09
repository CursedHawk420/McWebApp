using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class LuckPermsApi : Controller
    {
        private readonly LuckPermsService _luckPermsService;

        public LuckPermsApi(LuckPermsService luckPermsService)
        {
            _luckPermsService = luckPermsService;
        }

        [AllowAnonymous]
        [HttpGet("api/luckperms/data/{name}")]
        public async Task<IActionResult> GetLuckpermsUserData(string name)
        {
            var user = await _luckPermsService.GetUserAsync(await _luckPermsService.GetUserUuidAsync(name));
            return Json(user);
        }
    }
}
