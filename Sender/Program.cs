using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sending:");

            using (var connection = GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "message",
                        arguments: null,
                        exclusive: false,
                        autoDelete: false,
                        durable: false);

                    for (int i = 0; i < 15; i++)
                    {
                        var message = Encoding.UTF8.GetBytes($"Message {i}");

                        Thread.Sleep(1000);

                        channel.BasicPublish(exchange: "",
                            routingKey: "message",
                            basicProperties: null,
                            body: message);

                        Console.WriteLine(Encoding.Default.GetString(message));
                    }
                }
            }

            Console.ReadKey();
        }

        private static IConnection GetConnection()
            => new ConnectionFactory()
            {
                HostName = "localhost"
            }
            .CreateConnection();
    }
}
