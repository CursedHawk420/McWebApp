using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Channels;
using static Highgeek.McWebApp.Common.Services.ImageDetection;
using Microsoft.Extensions.Logging;

namespace Highgeek.McWebApp.Common.Services
{
    public class ImageCacheService
    {
        private readonly McWebApp1CmsContext _cmsContext;
        private readonly ILogger<ImageCacheService> _logger;

        public ImageCacheService(McWebApp1CmsContext cmsContext, ILogger<ImageCacheService> logger)
        {
            _cmsContext = cmsContext;
            _logger = logger;
        }

        public async Task<ImageCache> GetImageFromDatabase(string name)
        {
            ImageCache? image = await _cmsContext.ImageCache.FirstOrDefaultAsync(x => x.Name == name);
            if (name == "barrier")
            {
                image = await GetImageFromUrl("https://inventories.chasem.dev/assets/minecraft/barrier.png");
            }
            if (image == null)
            {
                if (name.Contains("_enchanted"))
                
                {
                    image = await GetImageFromUrl("https://inventories.chasem.dev/assets/minecraft/" + name + ".gif");
                }
                else
                {
                    image = await GetImageFromUrl("https://inventories.chasem.dev/assets/minecraft/" + name + ".png");
                }
            }
            return image;
        }

        public async Task<ImageCache> GetImageFromUrl(string url)
        {

            ImageCache? image = await _cmsContext.ImageCache.FirstOrDefaultAsync(s => s.Imageurl == url);
            if (image == null)
            {
                _logger.LogInformation("Caching new image on url: " + url);
                //string name = url.Split('.')[0];
                string name = url;
                name = name.Substring(name.LastIndexOf("/") + 1, name.LastIndexOf(".") - name.LastIndexOf("/") - 1).ToLower();

                _logger.LogInformation("Caching image: " + name + " on url: " + url);
                image = new ImageCache();
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                        ImageFormat format = ImageDetection.GetImageFormat(imageBytes);
                        if (format == ImageFormat.UNKNOWN)
                        {
                            image = await GetImageFromDatabase("barrier");
                            return image;
                        }
                        image.Format = format.ToString();
                        image.Uuid = Guid.NewGuid().ToString();
                        image.Name = name;
                        image.Image = imageBytes;
                        image.Imageurl = url;
                        image.Date = DateTime.Now.ToString();
                        await _cmsContext.AddAsync(image);
                        await _cmsContext.SaveChangesAsync();
                        return image;
                    }
                }
            }
            else
            {
                _logger.LogInformation("Image: " + image.Name + " already stored in database");
                return image;
            }
        }
    }

    public class ImageDetection
    {

        public enum ImageFormat
        {
            BMP,
            JPEG,
            GIF,
            TIFF,
            PNG,
            UNKNOWN
        }

        /// <summary>
        /// If a file exists, Read all bytes from it 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] LoadFileRawBytes(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return System.IO.File.ReadAllBytes(filePath);
            }
            else
                return null;
        }

        /// <summary>
        /// Load a file and determine what image type the file is
        /// This method needs to have enough free memory in order to load the whole file.
        /// Use other methods if you need more efficiency (eg load only the first four bytes of the file). - MC
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>ImageFormat</returns>
        public static ImageFormat GetImageFormat(string filePath)
        {
            byte[] fileBytes = LoadFileRawBytes(filePath);

            if (fileBytes == null)
            {
                return ImageFormat.UNKNOWN;
            }
            else
            {
                return GetImageFormat(fileBytes);
            }
        }

        /// <summary>
        /// Infer an Image type by looking at the first four bytes of a raw byte array
        /// Based on: https://stackoverflow.com/questions/210650/validate-image-from-file-in-c-sharp
        /// except that I already had the byte Array and didn't need the extra conversion to a stream
        /// I've checked bmp, gif, png, jpeg and tif but not tiff or jpeg2 (as I've no need for those) - MC
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns>ImageFormat</returns>
        public static ImageFormat GetImageFormat(byte[] byteArray)
        {

            const int INT_SIZE = 4; // We only need to check the first four bytes of the file / byte array.

            var bmp = System.Text.Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = System.Text.Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };                // PNG
            var tiff = new byte[] { 73, 73, 42 };                    // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                   // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };            // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };           // jpeg2 (canon)

            // Copy the first 4 bytes into our buffer 
            var buffer = new byte[INT_SIZE];
            System.Buffer.BlockCopy(byteArray, 0, buffer, 0, INT_SIZE);

            if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
                return ImageFormat.BMP;

            if (gif.SequenceEqual(buffer.Take(gif.Length)))
                return ImageFormat.GIF;

            if (png.SequenceEqual(buffer.Take(png.Length)))
                return ImageFormat.PNG;

            if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
                return ImageFormat.TIFF;

            if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
                return ImageFormat.TIFF;

            if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
                return ImageFormat.JPEG;

            if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
                return ImageFormat.JPEG;

            return ImageFormat.UNKNOWN;
        }

    }
}
