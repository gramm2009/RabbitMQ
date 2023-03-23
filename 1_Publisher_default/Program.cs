using System.Text;

namespace _1_Publisher_default
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //тестовый счетчик для визуализации
            int counter = 0;

            do
            {
                Thread.Sleep(1000);

                var factory = new ConnectionFactory { HostName = "localhost" };// работаем с очередью которая на локальной машине
                using (var connection = factory.CreateConnection())

                // создаем канал где выполняется основная работа с очередью
                using (var channel = connection.CreateModel())
                {
                    // обьявляем очередь - куда мы будем публиковать наше сообщение.
                    channel.QueueDeclare(queue: "test-queue", // Если такой очереди нет то она создастся
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    // Создали сообщение которое опубликуем в очередь.
                    string message = $"[{counter}]: Hello World!";

                    // Сообщение передается в массиве байт
                    byte[] body = Encoding.UTF8.GetBytes(message);

                    // Создали палблишера с "" (тип default) 
                    channel.BasicPublish(exchange: string.Empty, // С пустой строкой мы попадаем в ExchengeType (default)
                             routingKey: "test-queue", // по routing key мы свяжемся или создадим одноименную очередь.
                             basicProperties: null,
                             body: body);

                    Console.WriteLine($"Message is sent into RabbitMQ [N:{counter++}]");
                }

            } while (true);
        }
    }
}