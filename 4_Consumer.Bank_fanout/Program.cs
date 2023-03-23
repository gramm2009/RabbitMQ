using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace _4_Consumer.Bank_fanout
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // имя обменника , тип обменника
                // Одинаковая строка что в Producer что в Consumer
                channel.ExchangeDeclare(exchange: "notifier", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                    exchange: "notifier",
                    routingKey: string.Empty); // пустой ключ т.к. фаноут тип


                var consumer = new EventingBasicConsumer(channel);

                // Получаем смс Received
                consumer.Received += (sender, e) =>
                {
                    byte[] body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                };


                // подписка на очередь
                channel.BasicConsume(queue: queueName,
                                     autoAck: true, // извлекает смс из очереди (удаляет)
                                     consumer: consumer);

                Console.WriteLine($"Subscribed to the queue '{queueName}'");
                Console.ReadLine();
            }
        }
    }
}