using Highgeek.McWebApp.Common.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public class RedisService
    {
        public static readonly string host = ConfigProvider.Instance.GetConfigString("RedisOptions:Ip");


        public static readonly ConnectionMultiplexer Redis = ConnectionMultiplexer.Connect(host);

        public static readonly IDatabase Database = Redis.GetDatabase();

        public static readonly IServer server = Redis.GetServers().Single();

        public static async Task<string> GetFromRedisAsync(string uuid)
        {
            return await Database.StringGetAsync(uuid);
        }
        public static string GetFromRedis(string uuid)
        {
            return Database.StringGet(uuid);
        }

        public static async Task SetInRedis(string uuid, string value)
        {
            await Database.StringSetAsync(uuid, value);
        }

        public static async Task<List<string>> GetKeysList(string pattern)
        {
            List<string> output = new List<string>();

            foreach (var key in server.Keys(pattern: pattern))
            {
                output.Add(key);
            }
            return output;
        }

        public static async Task DelFromRedis(string uuid)
        {
            await Database.KeyDeleteAsync(uuid);
        }
    }
}
