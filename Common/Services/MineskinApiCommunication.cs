using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenApi.Highgeek.MineSkinApi;
using OpenApi.Highgeek.MineSkinApi.Api;
using OpenApi.Highgeek.MineSkinApi.Client;
using OpenApi.Highgeek.MineSkinApi.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Highgeek.McWebApp.Common.Helpers;

namespace Highgeek.McWebApp.Common.Services
{
    public class MineskinApiCommunication
    {
        private readonly Configuration _configuration = new Configuration();
        private readonly ILogger<MineskinApiCommunication> _logger;
        private string userAgent;  // string | Custom User-Agent for your application, see [user-agent.dev](https://user-agent.dev/) for implementation examples

        public MineskinApiCommunication(ILogger<MineskinApiCommunication> logger)
        {
            _logger = logger;
            _configuration.BasePath = "https://api.mineskin.org";
            userAgent = "mcwebapp/1.0";
            _configuration.UserAgent = userAgent;
            _configuration.AddApiKey(ConfigProvider.Instance.GetConfigString("MineSkinApi:ApiKey"), ConfigProvider.Instance.GetConfigString("MineSkinApi:ApiSecret"));
        }

        public SkinInfo GetTextureId(int mineskinid)
        {

            var apiInstance = new GetApi(_configuration);
            var id = mineskinid;  // decimal | 
            try
            {
                SkinInfo result = apiInstance.GetIdIdGet(id, userAgent);
                Debug.WriteLine(result);
                return result;
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GetApi.GetIdIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);

                return null;
            }
        }
        public GenerateUrlPost200Response UploadSkin(IFormFile file)
        {

            var apiInstance = new GenerateApi(_configuration);
            string model = "steve";  // string |  (optional)  (default to steve)
            string variant = "Classic";  // string | Skin variant - automatically determined based on the image if not specified (optional) 
            var name = "name_example";  // string |  (optional) 
            var visibility = 1;  // int? | Visibility of the generated skin. 0 for public, 1 for private (optional)  (default to 0)

            try
            {
                MemoryStream memStream = new MemoryStream();
                file.CopyTo(memStream);
                GenerateUrlPost200Response result = apiInstance.GenerateUploadPost(userAgent, null, model, variant, name, visibility, memStream);
                Debug.WriteLine(result);
                return result;
                //GenerateUrlPost200Response result = apiInstance.GenerateUploadPost(userAgent, model, variant, name, visibility, file);
                //Debug.WriteLine(result);
                //return result;
            }
            catch (ApiException e)
            {
                return null;
            }
        }

        public GenerateUrlPost200Response UrlSkin(string url)
        {
            var apiInstance = new GenerateApi(_configuration);
            var generateUrlPostRequest = new GenerateUrlPostRequest(); // GenerateUrlPostRequest | 
            generateUrlPostRequest.Url = url;

            try
            {
                GenerateUrlPost200Response result = apiInstance.GenerateUrlPost(userAgent, generateUrlPostRequest);
                if (!result.Variant.HasValue)
                {
                    result.Variant = GenerateUrlPost200Response.VariantEnum.Classic;
                }
                _logger.LogWarning("Succes");
                return result;
            }
            catch (ApiException e)
            {
                _logger.LogWarning("Mineskin error \nMessage: " + e.Message + "\nStacktrace: \n" + e.StackTrace);
                return null;
            }
        }
    }
}
