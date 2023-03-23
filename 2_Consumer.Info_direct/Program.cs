namespace _2_Consumer.Info_direct
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void Main(string[] args)
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    // Одинаковая строка что в Producer что в Consumer
                    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                        exchange: "direct_logs",
                        routingKey: "info");


                    var consumer = new EventingBasicConsumer(channel);

                    // Получаем смс Received
                    consumer.Received += (sender, e) =>
                    {
                        byte[] body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] Received {message}");
                    };


                    channel.BasicConsume(queue: queueName,
                                         autoAck: true, // извлекает смс из очереди (удаляет)
                                         consumer: consumer);

                    Console.WriteLine($"Subscribed to the queue '{queueName}'");
                    Console.ReadLine();
                }
            }
        }
    }
}