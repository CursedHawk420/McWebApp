using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MineSkinApi;
using MineSkinApi.Api;
using MineSkinApi.Client;
using MineSkinApi.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Highgeek.McWebApp.Common.Services
{
    public class MineskinApiCommunication
    {
        private readonly Configuration _configuration;
        private string userAgent;  // string | Custom User-Agent for your application, see [user-agent.dev](https://user-agent.dev/) for implementation examples

        public MineskinApiCommunication(UserManager<ApplicationUser> userManager, McserverMaindbContext mainDbContext, Configuration config)
        {
            config.BasePath = "https://api.mineskin.org";
            _configuration = config;
            userAgent = "";
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
                GenerateUrlPost200Response result = apiInstance.GenerateUploadPost(userAgent, model, variant, name, visibility, memStream);
                Debug.WriteLine(result);
                return result;
                //GenerateUrlPost200Response result = apiInstance.GenerateUploadPost(userAgent, model, variant, name, visibility, file);
                //Debug.WriteLine(result);
                //return result;
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GenerateApi.GenerateUploadPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
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
                Debug.WriteLine(result);
                return result;
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GenerateApi.GenerateUrlPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
                return null;
            }
        }
    }
}
