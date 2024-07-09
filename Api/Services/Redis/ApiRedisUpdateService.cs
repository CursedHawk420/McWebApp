﻿using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Services.Redis;
using Highgeek.McWebApp.Common.Services;

namespace Highgeek.McWebApp.Api.Services.Redis
{
    public interface IApiRedisUpdateService
    {
        public void Send(string stringToAdd);
    }
    public class ApiRedisUpdateService : IApiRedisUpdateService
    {
        private readonly LuckPermsService _luckPermsService;
        private readonly ILogger<ApiRedisUpdateService> _logger;

        public ApiRedisUpdateService(LuckPermsService luckPermsService, ILogger<ApiRedisUpdateService> logger)
        {
            _luckPermsService = luckPermsService;
            _logger = logger;
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
                case "prechat":
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    {
                        await PrechatEvent(Uuid);
                    }
                    return;
                default:
                    return;
            }
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
