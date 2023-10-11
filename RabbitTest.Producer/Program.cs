namespace RabbitTest.Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args == null || args.Length < 1)
                return;

            var filepath = args[0];

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => cts.Cancel();

            using var client = new HttpClient();
            using var rabbit = new RabbitService();
            var sender = rabbit.GetQueueSender();
            await foreach (var line in File.ReadLinesAsync(filepath, cts.Token))
                sender.SendMessage(line);
        }
    }
}