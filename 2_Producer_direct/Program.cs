using System.Text;

namespace _2_Producer_direct
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void Main(string[] args)
            {
                // Создали 3  Publisher c разными routingKey
                Task.Run(CreateTask(3000, "error"));
                Task.Run(CreateTask(3000, "info"));
                Task.Run(CreateTask(3000, "warning"));

                Console.ReadKey();

                static Func<Task> CreateTask(int timeToSleep, string routingKey)
                {
                    return () =>
                    {
                        int counter = 0;

                        do
                        {
                            Thread.Sleep(timeToSleep);

                            var factory = new ConnectionFactory { HostName = "localhost" };
                            using var connection = factory.CreateConnection();
                            using (var channel = connection.CreateModel())
                            {
                                // другой метод не как в 1 примере.
                                // Одинаковая строка что в Producer что в Consumer
                                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                                string message = $"Message type [{routingKey}] from publisher N {counter}";
                                var body = Encoding.UTF8.GetBytes(message);

                                // Создали палблишера с именем direct_logs (тип direct указан выше) 
                                channel.BasicPublish(exchange: "direct_logs",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);

                                Console.WriteLine($"Message type [{routingKey}] is sent into Direct Exchange [N:{counter++}]");
                            }

                        } while (true);
                    };
                }

            }
        }
    }
}