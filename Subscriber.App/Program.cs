using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");

        var subscriber = connectionMultiplexer.GetSubscriber();

        var channel = new RedisChannel("pubsub", RedisChannel.PatternMode.Auto);

        subscriber.Subscribe(channel, (channel, message) =>
        {
            Console.WriteLine(message);
        });

        Console.ReadLine();
    }
}