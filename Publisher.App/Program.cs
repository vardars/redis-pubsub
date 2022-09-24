using StackExchange.Redis;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");

        ISubscriber subscriber = connectionMultiplexer.GetSubscriber();

        var channel = new RedisChannel("pubsub", RedisChannel.PatternMode.Auto);

        await SchedulePublishMessage(subscriber, channel);
    }

    private static async Task SchedulePublishMessage(ISubscriber subscriber, RedisChannel channel)
    {
        var timer  = new PeriodicTimer(TimeSpan.FromSeconds(5));
        while (await timer.WaitForNextTickAsync())
        {
            subscriber.Publish(channel, $"Time is: {DateTime.Now}", CommandFlags.FireAndForget);
        }
    }
}