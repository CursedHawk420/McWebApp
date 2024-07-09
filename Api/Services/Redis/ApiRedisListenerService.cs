using Highgeek.McWebApp.Common.Services.Redis;
using StackExchange.Redis;

namespace Highgeek.McWebApp.Api.Services.Redis
{
    public class ApiRedisListenerService : BackgroundService
    {
        private readonly ILogger<ApiRedisListenerService> _logger;
        private readonly IApiRedisUpdateService _redisUpdateService;
        public ConnectionMultiplexer _redisConnection { get; set; }
        public ISubscriber _subscriber { get; set; }

        public ApiRedisListenerService(ILogger<ApiRedisListenerService> logger, IApiRedisUpdateService redisUpdateService)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            _redisConnection = RedisService.Redis;
            _subscriber = _redisConnection.GetSubscriber();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                return Task.Run(() => RedisListenerAsync(stoppingToken));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute RedisListenerService with exception message {ex.Message}. Good luck next round!\n Stacktrace: \n{ex.StackTrace}");
                return Task.CompletedTask;
            }
        }

        public async Task RedisListenerAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Redis Listener Service is starting.");


            await _subscriber.SubscribeAsync("*", async (channel, message) =>
            {
                var key = GetKey(channel);
                _logger.LogInformation("Key: " + key + " channel: " + channel + " message: " + message);
                if (key == "set")
                {

                    _redisUpdateService.Send(message);
                }
            });

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
    }
}
