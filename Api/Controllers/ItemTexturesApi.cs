using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ItemTexturesApi : Controller
    {
        private readonly ImageCacheService _imageCacheService;

        public ItemTexturesApi(ImageCacheService imageCacheService)
        {
            _imageCacheService = imageCacheService;
        }

        [AllowAnonymous]
        [HttpGet("api/images/items/url/{url}")]
        public async Task<IActionResult> GetUrlImage(string url)
        {
            ImageCache image = await _imageCacheService.GetImageFromUrl(url);
            return File(image.Image, "image/" + image.Format.ToLower());
        }

        [AllowAnonymous]
        [HttpGet("api/images/items/name/{name}")]
        public async Task<IActionResult> GetNameImage(string name)
        {
            ImageCache image = await _imageCacheService.GetImageFromDatabase(name);
            return File(image.Image, "image/" + image.Format.ToLower());
        }
    }
}
