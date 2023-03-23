using RabbitMQ.Client;
using System.Text;

namespace _4_Producer_fanout
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random= new Random();

            do
            {
                Thread.Sleep(2000);

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using (var channel = connection.CreateModel())
                {
                    // Одинаковая строка что в Producer что в Consumer
                    channel.ExchangeDeclare(exchange: "notifier", type: ExchangeType.Fanout);

                    double moneyCount = random.Next(1000, 10000);
                    string message = $"Send money {moneyCount}$";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "notifier",
                             routingKey: string.Empty, // убрали routingKey т.к. этот обменник не использует этот параметр.
                             basicProperties: null,
                             body: body);

                    Console.WriteLine($"Message type [fanout] is sent into Direct Exchange [N:{moneyCount}]");
                }

            } while (true);
        }
    }
}