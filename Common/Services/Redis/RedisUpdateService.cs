using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Models.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public interface IRedisUpdateService
    {
        public void Send(string stringToAdd);

        public event EventHandler<InventoryPositionInfo> InventoryChanged;

        public event EventHandler<RedisChatEntryAdapter> ChatChanged;

        public event EventHandler<RedisChatEntryAdapter> PreChatChanged;

        public event EventHandler<string> OtherRedisSetChange;

        public event EventHandler<string> SettingsChanged;
    }

    public class RedisUpdateService : IRedisUpdateService
    {
        private readonly ILogger<RedisUpdateService> _logger;
        private readonly LuckPermsService _luckPermsService;

        public event EventHandler<InventoryPositionInfo> InventoryChanged;
        public event EventHandler<RedisChatEntryAdapter> ChatChanged;
        public event EventHandler<RedisChatEntryAdapter> PreChatChanged;
        public event EventHandler<string> OtherRedisSetChange;
        public event EventHandler<string> SettingsChanged;

        public RedisUpdateService(ILogger<RedisUpdateService> logger, LuckPermsService luckPermsService)
        {
            _logger = logger;
            _luckPermsService = luckPermsService;
        }

        private string Uuid { get; set; }

        public async void Send(string stringToAdd)
        {
            Uuid = stringToAdd;
            string type;

            try
            {
                type = Uuid.Substring(0, Uuid.IndexOf(":"));
            }
            catch (Exception ex)
            {
                //Unable to parse type, some minecraft java plugin caused this
                _logger.LogWarning("Failed to parse Uuid of redis set event");
                return;
            }

            switch (type)
            {
                case "vinv" or "winv":
                    await InventoryEvent(Uuid, type);
                    return;
                case "chat":
                    await ChatEvent(Uuid);
                    return;
                case "prechat":
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    {
                        await PrechatEvent(Uuid);
                    }
                    return;
                case "settings":
                    await SettingsEvent(Uuid);
                    return;
                default:
                    OtherRedisSetChange?.Invoke(this, Uuid);
                    return;
            }

        }

        public async Task InventoryEvent(string uuid, string type)
        {
            InventoryPositionInfo positionInfo = new InventoryPositionInfo
            {
                rawuuid = uuid,
                type = type,
                position = uuid.Substring(uuid.LastIndexOf(":") + 1, uuid.Length - uuid.LastIndexOf(":") - 1),
                uuid = uuid.Substring(uuid.LastIndexOf(":") - 36, 36),
                Item = await RedisService.GetFromRedis(uuid)
            };

            InventoryChanged?.Invoke(this, positionInfo);
        }

        public async Task ChatEvent(string uuid)
        {
            string json = await RedisService.GetFromRedis(uuid);
            if (json == null) return;
            RedisChatEntryAdapter chatEntry = RedisChatEntryAdapter.FromJson(json);

            ChatChanged?.Invoke(this, chatEntry);
        }

        public async Task PrechatEvent(string uuid)
        {
            string json = await RedisService.GetFromRedis(uuid);
            if (json == null) return;
            RedisChatEntryAdapter chatEntry = RedisChatEntryAdapter.FromJson(json);

            PreChatChanged?.Invoke(this, chatEntry);
        }

        public async Task SettingsEvent(string uuid)
        {
            _logger.LogWarning("Invokig SettingsChanged: " + Uuid);
            SettingsChanged?.Invoke(this, uuid);
        }

    }
}
