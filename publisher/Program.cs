using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

Random random = new Random();
const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

channel.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

while (true)
{
    // var s = Console.ReadLine();
    //
    // if (string.IsNullOrWhiteSpace(s))
    //     continue;
    //
    // if (s == "-1")
    //     Environment.Exit(0);

    var s = new string(Enumerable.Repeat(chars, 6)
        .Select(s => s[random.Next(s.Length)]).ToArray());

    var body = Encoding.UTF8.GetBytes(s);

    channel.BasicPublish(exchange: string.Empty,
        routingKey: "hello",
        basicProperties: null,
        body: body);

    Console.WriteLine($" [x] Sent {s}");
    await Task.Delay(random.Next(25, 100));
}