using Highgeek.McWebApp.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services.Redis
{
    public class RedisListenerService : BackgroundService
    {
        private readonly ILogger<RedisListenerService> _logger;
        private readonly IRedisUpdateService _redisUpdateService;
        public ConnectionMultiplexer _redisConnection { get; set; }
        public ISubscriber _subscriber { get; set; }
        //public IDatabase _database { get; set; }

        public RedisListenerService(ILogger<RedisListenerService> logger, IRedisUpdateService redisUpdateService)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            _redisConnection = RedisService.Redis;
            _subscriber = _redisConnection.GetSubscriber();
            //_database = RedisService.Database;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                return Task.Run(() => RedisListenerAsync(stoppingToken));
            }
            catch (Exception ex)
            {
                ex.WriteExceptionToRedis();
                _logger.LogInformation(
                    $"Failed to execute RedisListenerService with exception message {ex.Message}. Good luck next round!\n Stacktrace: \n{ex.StackTrace}");
                return Task.CompletedTask;
            }
        }

        public async Task RedisListenerAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Redis Listener Service is starting.");

            //stoppingToken.Register(() => _logger.LogInformation($"Redis Listener Service background task is stopping."));

            // Subscribe to channels
            /*
            await _subscriber.SubscribeAsync("*", async (channel, message) =>
            {
                var key = GetKey(channel);
                _logger.LogInformation("Key: " + key + " channel: " + channel + " message: " + message);
                if (key == "set")
                {
                    //_logger.LogInformation("RedisListenerAsync triggers VinvUpdatedEventArgs with uuid: " + message);

                    _redisUpdateService.Send(message);
                }
            });*/

            await _subscriber.SubscribeAsync("*", async (channel, message) =>
            {
                var key = GetKey(channel);
                //_logger.LogInformation("Key: " + key + " channel: " + channel + " message: " + message);
                if (key == "set")
                {
                    //_logger.LogInformation("RedisListenerAsync triggers VinvUpdatedEventArgs with uuid: " + message);

                    _redisUpdateService.Send(message);
                }
            });

            //_logger.LogDebug($"Redis Listener Service RESTARTING.");

            //await Task.Run(() => RedisListenerAsync(stoppingToken));

            await Task.Delay(Timeout.Infinite, stoppingToken);

            _logger.LogDebug($"Redis Listener Service is stopping.");
        }

        public string GetKey(string channel)
        {
            var index = channel.IndexOf(':');

            if (index >= 0 && index < channel.Length - 1)
            {
                return channel[(index + 1)..];
            }

            return channel;
        }

        /*public async Task<string> getFromRedis(string uuid)
        {
            return await _database.StringGetAsync(uuid);
        }

        public async Task setInRedis(string uuid, string value)
        {
            await _database.StringSetAsync(uuid, value);
        }*/
    }
}
