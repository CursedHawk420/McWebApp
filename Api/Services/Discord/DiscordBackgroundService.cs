using Discord;
using Discord.Commands;
using Discord.Webhook;
using Discord.WebSocket;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.AspNetCore;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.X509;

namespace Highgeek.McWebApp.Api.Services.Discord
{
    public class DiscordBackgroundService : BackgroundService
    {
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly LuckPermsService _luckPermsService;
        public readonly DiscordSocketClient _client;
        public readonly CommandService _command;
        private readonly ILogger _logger;
        private readonly protected string _botToken;
        private readonly protected ulong _guildId;
        public SocketGuild _guild;
        private bool cacheMessages = true;
        private List<RedisChatEntryAdapter> cachedMessages = new List<RedisChatEntryAdapter>();

        public List<ChannelSettingsAdapter> channelSettings = new List<ChannelSettingsAdapter> { };

        private readonly Dictionary<string, ulong> redisMap = new Dictionary<string, ulong>();
        private readonly Dictionary<ulong, string> discordMap = new Dictionary<ulong, string>();

        public DiscordBackgroundService(ILogger<DiscordBackgroundService> logger, IRedisUpdateService apiRedisUpdateService, CommandService commandService, LuckPermsService luckPermsService)
        {
            _botToken = ConfigProvider.Instance.GetConfigString("DiscordAuthOptions:DiscordBotToken");
            _guildId = Convert.ToUInt64(ConfigProvider.Instance.GetConfigString("DiscordAuthOptions:DiscordGuildId"));
            _redisUpdateService = apiRedisUpdateService;
            _luckPermsService = luckPermsService;

            _logger = logger;
            _command = commandService;
            _client = new DiscordSocketClient();

            redisMap = ConfigProvider.Instance.GetConfigurationManager().GetSection("DiscordChannelBinding").GetChildren().ToDictionary(x => x.Key, x => Convert.ToUInt64(x.Value));
            discordMap = ConfigProvider.Instance.GetConfigurationManager().GetSection("DiscordChannelBinding").GetChildren().ToDictionary(x => Convert.ToUInt64(x.Value), x => x.Key);

            LoadChannelSettings(null, null);


            _redisUpdateService.ChatChanged += SendChatMessageToDiscord;
            _redisUpdateService.SettingsChanged += LoadChannelSettings;
            _redisUpdateService.PreChatChanged += LegacyPreChatEvent;
        }

        public async void LoadChannelSettings(object sender, string uuid)
        {
            _logger.LogInformation($"Trying to load settigns");
            if (uuid.IsNullOrEmpty() || uuid.StartsWith("settings:server:chat:channels"))
            {
                channelSettings.Clear();
                _logger.LogInformation($"Loading channel settings from redis.");
                foreach (var key in RedisService.GetKeysList("settings:server:chat:channels:*").Result)
                {
                    channelSettings.Add(ChannelSettingsAdapter.FromJson(RedisService.GetFromRedis(key).Result));
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                return Task.Run(() => DiscordSocketListener(stoppingToken));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute DiscordBackgroundService with exception message {ex.Message}. Good luck next round!\n Stacktrace: \n{ex.StackTrace}");
                return Task.CompletedTask;
            }
        }

        public async Task DiscordSocketListener(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"DiscordBackgroundService is starting.");

            _client.Log += Log;
            _client.MessageReceived += MessageFromDiscordReceived;
            _client.GuildAvailable += GuildAvaiable;
            _client.GuildUnavailable += GuildUnavailable;

            await _client.LoginAsync(TokenType.Bot, _botToken);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite, stoppingToken);

            _logger.LogDebug($"DiscordBackgroundService is stopping.");

            _client.Log -= Log;
            _client.MessageReceived -= MessageFromDiscordReceived;
            _client.GuildAvailable -= GuildAvaiable;
            _client.GuildUnavailable -= GuildUnavailable;
            _client.Dispose();
        }


        private Task Log(LogMessage message)
        {
            _logger.LogInformation($"DiscordBackgroundService LogMessage: " + message.Message);
            return Task.CompletedTask;
        }

        private Task GuildUnavailable(SocketGuild socketGuild)
        {
            if(socketGuild.Id == _guild.Id)
            {
                cacheMessages = true;
            }
            return Task.CompletedTask;
        }

        private Task GuildAvaiable(SocketGuild socketGuild)
        {
            if (socketGuild.Id == _guildId)
            {
                cacheMessages = false;
                _guild = socketGuild;
                ProcessCachedMessages();

            }
            return Task.CompletedTask;
        }

        private async Task ProcessCachedMessages()
        {
            foreach (var message in cachedMessages)
            {
                SendChatMessageToDiscord(this, message);
            }
        }

        //discord bot receives some message from discord
        public async Task MessageFromDiscordReceived(SocketMessage socketMessage)
        {
            //_logger.LogInformation($"DiscordSocketListener MessageReceived SOURCE: " + socketMessage.Source.ToString());
            //_logger.LogInformation($"DiscordSocketListener MessageReceived ID: " + socketMessage.Author.Id.ToString());
            if (socketMessage.Source == MessageSource.Webhook)
            {
                return;
            }
            if (discordMap[socketMessage.Channel.Id] == null)
            {
                return;
            }

            try
            {
                IMessage message = await _guild.GetTextChannel(socketMessage.Channel.Id).GetMessageAsync(socketMessage.Id);
                _logger.LogInformation($"DiscordSocketListener MessageReceived: " + message.Content);
                RedisChatEntryAdapter redisChat = new RedisChatEntryAdapter();

                redisChat.Channel = discordMap[message.Channel.Id];
                ChannelSettingsAdapter channelSetting = channelSettings.FirstOrDefault(x => x.Name == redisChat.Channel);

                if(channelSetting == null)
                {
                    return;
                }

                redisChat.Source = "discord";
                redisChat.Servername = "discord";
                redisChat.PrettyServerName = "&2Disc";
                redisChat.Channelprefix = channelSetting.Prefix;
                redisChat.Datetime = DateTimeOffset.Now;
                redisChat.Message = message.Content;


                /*
                todo: linking discord with web and mc account
                redisChat.Suffix;
                redisChat.Prefix;
                redisChat.PlayerUuid;
                redisChat.Primarygroup;
                redisChat.Username;
                redisChat.Nickname;
                 */

                redisChat.Prefix = "§6[Pleb] §f";
                redisChat.Suffix = "&f";
                redisChat.Primarygroup = "default";
                redisChat.Nickname = message.Author.Username;
                redisChat.Username = message.Author.Username;

                redisChat.Uuid = "chat:" + redisChat.Channel + ":" + redisChat.Datetime + ":" + redisChat.Username;

                await RedisService.SetInRedis(redisChat.Uuid, redisChat.ToJson());


                _logger.LogInformation($"DiscordSocketListener MessageReceived channel id: " + discordMap[message.Channel.Id]);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DiscordSocketListener MessageReceived exception: \n" + ex.Message);
            }

        }



        //discord bot sends message from other sources
        public async void SendChatMessageToDiscord(object sender, RedisChatEntryAdapter chatEntryAdapter)
        {
            if (!cacheMessages)
            {
                if (chatEntryAdapter.Source != "discord")
                {
                    try
                    {
                        await BuildWebhookMessage(chatEntryAdapter);
                        _logger.LogInformation("keyValuePairs[chatEntryAdapter.Channel]: " + redisMap[chatEntryAdapter.Channel].ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"DiscordSocketListener SendChatMessageToDiscord exception: \n" + ex.Message);
                    }
                }
            }
            else
            {
                cachedMessages.Add(chatEntryAdapter);
            }
        }

        public async Task BuildWebhookMessage(RedisChatEntryAdapter chatEntry)
        {
            var channel = await _client.GetChannelAsync(redisMap[chatEntry.Channel]);
            var webhook = await GetWebhook(channel as IIntegrationChannel);
            await ExecuteWebhook(chatEntry, webhook);

        }


        public async Task ExecuteWebhook(RedisChatEntryAdapter chatEntry, IWebhook webhook)
        {

            var webhookClient = new DiscordWebhookClient(webhook);

            string random = new UUID().ToString();
            string avatarUrl = "https://api.highgeek.eu/api/skins/playerhead/" + chatEntry.Username + "?" + random;

            var messageId = await webhookClient.SendMessageAsync(text: chatEntry.Message, avatarUrl: avatarUrl, username: chatEntry.Nickname);

            _logger.LogInformation($"DiscordSocketListener ExecuteWebhook: \n" + messageId.ToString());

        }

        public async Task<IWebhook> GetWebhook(IIntegrationChannel channel)
        {
            foreach (var hook in _guild.GetWebhooksAsync().Result)
            {
                if (hook != null)
                {
                    if (hook.Name == "McWebAppWebhook " + channel.Id)
                    {
                        return hook;
                    }
                }
            }
            return await CreateWebhook(channel);
        }

        public async Task<IWebhook> CreateWebhook(IIntegrationChannel channel)
        {
            return await channel.CreateWebhookAsync("McWebAppWebhook " + channel.Id);
        }



        public async void LegacyPreChatEvent(object sender, RedisChatEntryAdapter chatEntry)
        {
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

            await RedisService.DelFromRedis(chatEntry.Uuid);
            chatEntry.Uuid = chatEntry.Uuid.Replace("prechat", "chat");

            await RedisService.SetInRedis(chatEntry.Uuid, chatEntry.ToJson());
        }
    }
}
