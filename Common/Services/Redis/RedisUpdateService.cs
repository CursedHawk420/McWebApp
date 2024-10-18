using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter;
using Highgeek.McWebApp.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Highgeek.McWebApp.Common.Helpers;
using MimeKit.Encodings;

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

        public event EventHandler<string> PlayersSettingsChanged;

        public event EventHandler<string> PlayersListChanged;

        public event EventHandler<string> PlayersEconomyChanged;

        public event EventHandler<string> LuckpermsChanged;

        public event EventHandler<string> McAccountDisconnectUpdate;

        public event EventHandler<string> SessionListUpdate;


        event Action LocaleChangeRequested;
        void CallLocaleChange();

        event Action LanguageProviderRefreshRequested;
        void CallLanguageProviderRefresh();

        event Action ServerListRefreshRequested;
        void CallServerListRefresh();
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
        public event EventHandler<string> PlayersSettingsChanged;
        public event EventHandler<string> LuckpermsChanged;
        public event EventHandler<string> PlayersListChanged;
        public event EventHandler<string> PlayersEconomyChanged;
        public event EventHandler<string> McAccountDisconnectUpdate;
        public event EventHandler<string> SessionListUpdate;

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
                ex.WriteExceptionToRedis();
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
                case "luckperms":
                    await LuckpermsEvent(Uuid);
                    return;
                case "players":
                    await PlayersEvent(Uuid);
                    return;
                case "server":
                    await ServerListEvent(Uuid);
                    return;
                case "economy":
                    await PlayerEconomyEvent(Uuid);
                    return;
                case "appchannel":
                    await AppChannelMessageEvent(Uuid);
                    return;
                default:
                    OtherRedisSetChange?.Invoke(this, Uuid);
                    return;
            }

        }

        public async Task AppChannelMessageEvent(string uuid)
        {
            if (uuid.Contains("mcwebapp"))
            {
                if (uuid.Contains("disconnectmcuser"))
                {
                    McAccountDisconnectUpdate?.Invoke(this, await RedisService.GetFromRedisAsync(uuid));
                }
                if (uuid.Contains("sessionlist"))
                {
                    SessionListUpdate?.Invoke(this, await RedisService.GetFromRedisAsync(uuid));
                }
            }
        }


        public async Task PlayerEconomyEvent(string uuid)
        {
            if (uuid.Contains("players"))
            {
                PlayersEconomyChanged?.Invoke(this, uuid);
            }
        }

        public async Task ServerListEvent(string uuid)
        {
            if (uuid.Contains("playerlist"))
            {
                try
                {
                    PlayersListChanged?.Invoke(this, uuid);
                }
                catch (Exception ex)
                {
                    ex.WriteExceptionToRedis();
                    return;
                }
            }
        }

        public async Task PlayersEvent(string uuid)
        {
            if (uuid.Contains("settings"))
            {
                try
                {
                    PlayersSettingsChanged?.Invoke(this, uuid);
                }
                catch (Exception ex)
                {
                    ex.WriteExceptionToRedis();
                    return;
                }
            }
        }

        public async Task LuckpermsEvent(string uuid)
        {
            if (uuid.StartsWith("luckperms:log:toresolve:"))
            {
                LuckpermsChanged?.Invoke(this, uuid);
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
                Item = await RedisService.GetFromRedisAsync(uuid)
            };

            InventoryChanged?.Invoke(this, positionInfo);
        }

        public async Task ChatEvent(string uuid)
        {
            string json = await RedisService.GetFromRedisAsync(uuid);
            if (json == null) return;
            RedisChatEntryAdapter chatEntry = RedisChatEntryAdapter.FromJson(json);

            ChatChanged?.Invoke(this, chatEntry);
        }

        public async Task PrechatEvent(string uuid)
        {
            string json = await RedisService.GetFromRedisAsync(uuid);
            if (json == null) return;
            RedisChatEntryAdapter chatEntry = RedisChatEntryAdapter.FromJson(json);

            PreChatChanged?.Invoke(this, chatEntry);
        }

        public async Task SettingsEvent(string uuid)
        {
            if (uuid.Contains("language"))
            {
                CallLocaleChange();
            }
            else
            {
                _logger.LogWarning("Invokig SettingsChanged: " + Uuid);
                SettingsChanged?.Invoke(this, uuid);
            }
        }

        public event Action ServerListRefreshRequested;
        public void CallServerListRefresh()
        {
            ServerListRefreshRequested?.Invoke();
        }

        public event Action LocaleChangeRequested;
        public void CallLocaleChange()
        {
            LocaleChangeRequested?.Invoke();
        }


        public event Action LanguageProviderRefreshRequested;
        public void CallLanguageProviderRefresh()
        {
            LanguageProviderRefreshRequested?.Invoke();
        }
    }
}
