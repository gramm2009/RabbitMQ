namespace _1_Consumer_default
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "test-queue", //(Идентпатентный) если очередь не создана то он создаст его.
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                var consumer = new EventingBasicConsumer(channel);

                // Получаем смс Received
                consumer.Received += (model, e) =>
                {
                    byte[] body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                };

                // подписываемся на очередь
                channel.BasicConsume(queue: "test-queue",
                                     autoAck: true, // извлекает смс из очереди (удаляет)
                                     consumer: consumer);

                Console.WriteLine("Subscribed to the queue 'test-queue'");
                Console.ReadLine();
            }
        }
    }
}