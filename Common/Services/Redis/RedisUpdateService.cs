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

        public event EventHandler<string> OtherRedisSetChange;
    }

    public class RedisUpdateService : IRedisUpdateService
    {
        private readonly ILogger<RedisUpdateService> _logger;
        private readonly LuckPermsService _luckPermsService;

        public event EventHandler<InventoryPositionInfo> InventoryChanged;
        public event EventHandler<RedisChatEntryAdapter> ChatChanged;
        public event EventHandler<string> OtherRedisSetChange;

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
                    //await PrechatEvent(Uuid);
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
            //happens when mcserver receives discord guild message, we need to get player data before we send RedisService.SetInRedis() and goes to ChatEvent()
            string json = await RedisService.GetFromRedis(uuid);
            if (json == null) return;
            RedisChatEntryAdapter chatEntry = RedisChatEntryAdapter.FromJson(json);

            var LuckUser = await _luckPermsService.GetUserAsync(await _luckPermsService.GetUserUuidAsync(chatEntry.Username));
            if (LuckUser is not null)
            {
                chatEntry.Prefix = LuckUser.Metadata.Prefix;
                chatEntry.Suffix = LuckUser.Metadata.Suffix;
                chatEntry.PlayerUuid = LuckUser.UniqueId.ToString();
            }
            else
            {
                chatEntry.Prefix = "§6[Pleb] §f";
                chatEntry.Suffix = "§f";
                chatEntry.PlayerUuid = "00000000-0000-0000-0000-000000000000";
            }

            await RedisService.DelFromRedis(uuid);
            uuid = uuid.Replace("prechat", "chat");
            chatEntry.Uuid = uuid;

            await RedisService.SetInRedis(uuid, chatEntry.ToJson());
        }

    }
}
