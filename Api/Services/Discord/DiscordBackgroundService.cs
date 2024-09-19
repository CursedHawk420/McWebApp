using Discord;
using Discord.Commands;
using Discord.Webhook;
using Discord.WebSocket;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Helpers.Channels;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Adapters;
using Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Services.Redis;
using OpenApi.Highgeek.LuckPermsApi.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        private readonly McserverMaindbContext _mcMainContext;

        public List<ChannelSettingsAdapter> channelSettings = new List<ChannelSettingsAdapter> { };

        public List<DiscordPair> channelPairs = new List<DiscordPair> { };
        public List<DiscordPair> rolePairs = new List<DiscordPair> { };

        //private readonly Dictionary<string, ulong> redisMap = new Dictionary<string, ulong>();
        //private readonly Dictionary<ulong, string> discordMap = new Dictionary<ulong, string>();

        public DiscordBackgroundService(ILogger<DiscordBackgroundService> logger, IRedisUpdateService apiRedisUpdateService, CommandService commandService, LuckPermsService luckPermsService)
        {
            _botToken = ConfigProvider.GetConfigString("DiscordAuthOptions:DiscordBotToken");
            _guildId = Convert.ToUInt64(ConfigProvider.GetConfigString("DiscordAuthOptions:DiscordGuildId"));
            _redisUpdateService = apiRedisUpdateService;
            _luckPermsService = luckPermsService;
            _logger = logger;
            _command = commandService;
            DiscordSocketConfig config = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All
            };
            _client = new DiscordSocketClient(config);

            LoadSettings(null, null);

            _redisUpdateService.ChatChanged += SendChatMessageToDiscord;
            _redisUpdateService.SettingsChanged += LoadSettings;
            _redisUpdateService.PreChatChanged += LegacyPreChatEvent;
            _redisUpdateService.LuckpermsChanged += LuckpermsChangedEvent;
            _mcMainContext = GetMaindbContext();
        }

        private McserverMaindbContext GetMaindbContext()
        {
            return new McserverMaindbContext();
        }

        public async void LoadSettings(object sender, string uuid)
        {
            _logger.LogInformation($"Trying to load settigns");
            if (uuid.IsNullOrEmpty() || uuid.StartsWith("settings:server:chat:channels"))
            {
                channelSettings.Clear();
                _logger.LogInformation($"Loading channel settings from redis.");
                foreach (var key in RedisService.GetKeysList("settings:server:chat:channels:*").Result)
                {
                    channelSettings.Add(ChannelSettingsAdapter.FromJson(RedisService.GetFromRedisAsync(key).Result));
                }
            }

            if (uuid.IsNullOrEmpty() || uuid.Equals("settings:mcwebapp:discord:channels"))
            {
                _logger.LogInformation($"Loading discord channel settings pairs from redis.");
                DiscordPairing discordPairsList = DiscordPairing.FromJson(await RedisService.GetFromRedisAsync("settings:mcwebapp:discord:channels"));
                channelPairs = discordPairsList.DiscordPairs.ToList();
            }
            if (uuid.IsNullOrEmpty() || uuid.Equals("settings:mcwebapp:discord:roles"))
            {
                _logger.LogInformation($"Loading discord roles settings pairs from redis.");

                DiscordPairing discordPairsList = DiscordPairing.FromJson(RedisService.GetFromRedisAsync("settings:mcwebapp:discord:roles").Result);
                rolePairs = discordPairsList.DiscordPairs.ToList();
            }
        }



        public static DiscordPair GetMatchingDiscordId(List<DiscordPair> discordPairs, string toFind)
        {
            return discordPairs.FirstOrDefault(i => i.Name == toFind);
        }

        public static DiscordPair GetMatchingName(List<DiscordPair> discordPairs, ulong toFind)
        {
            return discordPairs.FirstOrDefault(i => i.DiscordId == toFind);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                return Task.Run(() => DiscordSocketListener(stoppingToken));
            }
            catch (Exception ex)
            {
                ex.WriteExceptionToRedis();
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
            if (socketMessage.Channel.GetChannelType() == ChannelType.DM && socketMessage.Content.Length == 6)
            {
                IUser user = socketMessage.Author;

                var statusCode = await CheckCodeAsync(socketMessage.Content.ToUpper());

                if (statusCode.Success){
                    await LinkAccount(user, (DiscordCode) statusCode.Object);
                }
            }
            if (GetMatchingName(channelPairs, socketMessage.Channel.Id) == null)
            {
                return;
            }

            try
            {
                IMessage message = await _guild.GetTextChannel(socketMessage.Channel.Id).GetMessageAsync(socketMessage.Id);
                IGuildUser guildUser = _guild.GetUser(message.Author.Id);
                _logger.LogInformation($"DiscordSocketListener MessageReceived: " + message.Content);
                RedisChatEntryAdapter redisChat = new RedisChatEntryAdapter();

                redisChat.Channel = GetMatchingName(channelPairs, message.Channel.Id).Name;
                ChannelSettingsAdapter channelSetting = channelSettings.FirstOrDefault(x => x.Name == redisChat.Channel);

                if(channelSetting == null)
                {
                    return;
                }
                DateTime dateTime = DateTime.UtcNow;

                string date = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF");

                redisChat.Source = "discord";
                redisChat.Servername = "discord";
                redisChat.PrettyServerName = "&2Disc";
                redisChat.Channelprefix = channelSetting.Prefix;
                redisChat.Datetime = dateTime;
                redisChat.Message = message.Content;

                redisChat.Username = message.Author.Username;

                if (guildUser.Nickname != null)
                {
                    redisChat.Nickname = guildUser.Nickname;
                }
                else
                {
                    redisChat.Nickname = guildUser.GlobalName;
                }

                var LuckUser = await GetDiscordUserLuckUser(guildUser);
                if (LuckUser is not null)
                {
                    redisChat.Prefix = LuckUser.Metadata.Prefix;
                    redisChat.Suffix = LuckUser.Metadata.Suffix;
                    redisChat.PlayerUuid = LuckUser.UniqueId.ToString();
                    redisChat.Primarygroup = LuckUser.Metadata.PrimaryGroup;
                }
                else
                {
                    redisChat.Prefix = "§6[Pleb] §f";
                    redisChat.Suffix = "§f";
                    redisChat.PlayerUuid = "00000000-0000-0000-0000-000000000000";
                    redisChat.Primarygroup = "default";
                }


                redisChat.Uuid = "chat:" + redisChat.Channel + ":" + date.Replace(":", "-") + ":" + redisChat.Nickname;

                await RedisService.SetInRedis(redisChat.Uuid, redisChat.ToJson());


                _logger.LogInformation($"DiscordSocketListener MessageReceived channel id: " + GetMatchingName(channelPairs, message.Channel.Id).Name);
                return;

            }
            catch (Exception ex)
            {
                ex.WriteExceptionToRedis();
                _logger.LogInformation($"DiscordSocketListener MessageReceived exception: \n" + ex.Message);
                return;
            }

        }

        private async Task<User> GetDiscordUserLuckUser(IUser guildUser)
        {
            DiscordAccount discordAccount = await _mcMainContext.DiscordAccounts.FirstOrDefaultAsync(x => x.Discord == guildUser.Id.ToString());
            if (discordAccount is not null)
            {
                return await _luckPermsService.GetUserAsync(discordAccount.Uuid);
            }
            return null;
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
                        _logger.LogInformation("keyValuePairs[chatEntryAdapter.Channel]: " + GetMatchingDiscordId(channelPairs, chatEntryAdapter.Channel).DiscordId);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteExceptionToRedis();
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
            var channel = await _client.GetChannelAsync(GetMatchingDiscordId(channelPairs, chatEntry.Channel).DiscordId);
            var webhook = await GetWebhook(channel as IIntegrationChannel);
            await ExecuteWebhook(RemoveColorCodes(chatEntry), webhook);

        }


        public static RedisChatEntryAdapter RemoveColorCodes(RedisChatEntryAdapter chatEntryAdapter)
        {
            string message = "";
            if (chatEntryAdapter.Message.Contains("&"))
            {
                string[] split = chatEntryAdapter.Message.Split("&");

                for (int i = 1; i < split.Length; i++)
                {
                    if(split[i].Length >= 1)
                    {
                        message += split[i].Remove(0, 1);
                    }
                }
                chatEntryAdapter.Message = message;
            }
            if (chatEntryAdapter.Message.Contains("§"))
            {
                string[] split = chatEntryAdapter.Message.Split("§");

                for (int i = 1; i < split.Length; i++)
                {
                    if (split[i].Length >= 1)
                    {
                        message += split[i].Remove(0, 1);
                    }
                }
                chatEntryAdapter.Message = message;
            }

            return chatEntryAdapter;
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

        public async void LuckpermsChangedEvent(object sender, LuckpermsRedisLogAdapter log)
        {
            //DiscordAccount discordAccount = await _mcMainContext.DiscordAccounts.FirstOrDefaultAsync(x => x.Uuid == log.TargetUuid.ToString());
            //await UpdateDiscordRoles(discordAccount);
        }

        public async Task UpdateDiscordRoles(DiscordAccount discordAccount)
        {
            User luckUser = await _luckPermsService.GetUserAsync(discordAccount.Uuid);
            
            var list = new List<ulong>();
            ulong Ulong = Convert.ToUInt64(discordAccount.Discord);
            list.Add(await TransformLuckpermsRoleToDiscord(luckUser.Metadata.PrimaryGroup));
            IGuildUser user = _guild.GetUser(Ulong);
            await user.ModifyAsync(x => {
                x.Nickname = discordAccount.Playername;
            });
            AddDiscordRoles(user, list);
        }

        public async Task<ulong> TransformLuckpermsRoleToDiscord(string role) 
        {
            return GetMatchingDiscordId(rolePairs, role).DiscordId;
        }

        public async Task AddDiscordRoles(IGuildUser user, List<ulong> rolesToAdd)
        {
            await (user as IGuildUser).AddRolesAsync(rolesToAdd);
        }


        public async Task<StatusModel> CheckCodeAsync(string code)
        {
            var discordCodeEntry = await _mcMainContext.DiscordCodes.FirstOrDefaultAsync(c => c.Code == code);
            if (discordCodeEntry is not null && discordCodeEntry.Code == code)
            {

                return new StatusModel("Successfully connected account!", discordCodeEntry);
            }
            else
            {
                return new StatusModel("Wrong linking code.", "Wrong linking code");
            }
        }

        public async Task LinkAccount(IUser discordUser, DiscordCode discordCode)
        {
            var xconomy = await _mcMainContext.Xconomies.FirstOrDefaultAsync(x => x.Uid == discordCode.Uuid);
            DiscordAccount discordAccount = new DiscordAccount();
            discordAccount.Discord = discordUser.Id.ToString();
            discordAccount.Uuid = discordCode.Uuid.ToString();
            discordAccount.Playername = xconomy.Player;


            await _mcMainContext.DiscordAccounts.AddAsync(discordAccount);
            await _mcMainContext.SaveChangesAsync();

            PlayerServerSettings playerServerSettings = JsonConvert.DeserializeObject<PlayerServerSettings>(await RedisService.GetFromRedisAsync("players:settings:" + xconomy.Player));
            playerServerSettings.hasConnectedDiscord = true;
            await RedisService.SetInRedis("players:settings:" + xconomy.Player, JsonConvert.SerializeObject(playerServerSettings));
            await UpdateDiscordRoles(discordAccount);
        }
    }
}
