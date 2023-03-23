namespace _3_Consumer.Ecological_topic
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
                    // имя обменника , тип обменника
                    // Одинаковая строка что в Producer что в Consumer
                    channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                        exchange: "topic_logs",
                        routingKey: "*.*.*.ecological"); // должно идти любые 3 слова потом ecological


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
                    Console.WriteLine($"Listening \"*.*.*.ecological\"");
                    Console.ReadLine();
                }
            }
        }
    }
}