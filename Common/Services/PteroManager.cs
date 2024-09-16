using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Identity;
using Sharpdactyl;
using Sharpdactyl.Models.Client;
using RestSharp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Helpers;

namespace Highgeek.McWebApp.Common.Services
{
    public class PteroManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly MineskinApiCommunication _mineskinApi;
        private readonly string hostName = "https://ptero.highgeek.eu";


        private readonly string apiKey = ConfigProvider.GetConfigString("PterodactylOptions:ApiKey");
        private readonly string bearerToken = ConfigProvider.GetConfigString("PterodactylOptions:BearerToken");

        public PteroManager(UserManager<ApplicationUser> userManager, McserverMaindbContext mainDbContext, MineskinApiCommunication mineskinApiCommunication)
        {
            //zjistit jestli je potřeba usermanager
            _userManager = userManager;
            _mcMainDbContext = mainDbContext;
            _mineskinApi = mineskinApiCommunication;
        }

        public async Task<List<ServerDatum>> GetServerList()
        {
            SharpDactyl sharpdactyl = new SharpDactyl(hostName, apiKey);
            return await sharpdactyl.GetServers();
        }

        public async Task<ServerDatum> GetServerById(string serverid)
        {
            SharpDactyl sharpdactyl = new SharpDactyl(hostName, apiKey);
            return await sharpdactyl.GetServerById(serverid);
        }

        public async Task<ServerUtil> GetServerUsage(string serverid)
        {
            SharpDactyl sharpdactyl = new SharpDactyl(hostName, apiKey);
            //ServerDatum srv = await sharpdactyl.Client.GetServerById(serverid);
            return await sharpdactyl.GetServerUsage(serverid);
        }

        public async Task<RestResponse> PostServerCommand(string server, string command)
        {
            var options = new RestClientOptions("https://ptero.highgeek.eu")
            {
                MaxTimeout = -1, 
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/client/servers/"+server+"/command", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", bearerToken);
            var body = "{\"command\":\"" + command + "\"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        public async Task<RestResponse> SendSignalToServer(string server, string signal)
        {
            /*SharpDactyl sharpdactyl = new SharpDactyl(hostName, apiKey);
            return await sharpdactyl.SendSignal(server, (Sharpdactyl.Enums.PowerSettings)signal);*/
            var options = new RestClientOptions("https://ptero.highgeek.eu")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/client/servers/f571789c/power", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", bearerToken);
            var body = "{\"signal\":\"" + signal + "\"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
