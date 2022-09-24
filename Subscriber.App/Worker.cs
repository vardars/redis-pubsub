using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Subscriber.App
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            var connectionMultiplexer = ConnectionMultiplexer.Connect("redis");

            var subscriber = connectionMultiplexer.GetSubscriber();

            var channel = new RedisChannel("pubsub", RedisChannel.PatternMode.Auto);

            subscriber.Subscribe(channel, (channel, message) =>
            {
                Console.WriteLine(message);
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            
            _logger.LogInformation($"Worker shutting at: {DateTimeOffset.Now}");
        }
    }
}
