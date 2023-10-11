using MySqlConnector;

namespace RabbitTest.Consumer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => cts.Cancel();

            using var client = new HttpClient();
            using var rabbit = new RabbitService();
            var listener = rabbit.GetQueueListener();
            var builder = new MySqlConnectionStringBuilder
            { 
                Server = "localhost",
                Database = "default",
                UserID = "root",
                Password = "example",
                SslMode = MySqlSslMode.None
            };
            await using var dbAccess = new MySqlAccess(builder.ConnectionString);
            await listener.Listen(async (item, cancellationToken) => 
            {
                var request = new HttpRequestMessage(HttpMethod.Get, item);
                var response = await client.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    var info = new InfoItem
                    { 
                        Url = item,
                        Date = DateTime.Now,
                        Response = content
                    };
                    await dbAccess.Save(info);
                }
                else
                    Console.WriteLine($"Unable to get content from {item}: {response.StatusCode}");
            }, cts.Token);
        }
    }
}