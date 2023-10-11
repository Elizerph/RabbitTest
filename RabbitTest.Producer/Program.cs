namespace RabbitTest.Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var filepath = "C:\\Users\\Aleksey\\Desktop\\urls.txt";

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