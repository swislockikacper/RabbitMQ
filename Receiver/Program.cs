using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Messages:");

            using (var connection = GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "message",
                        arguments: null,
                        exclusive: false,
                        durable: false,
                        autoDelete: false);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, eventArgs) =>
                    {
                        var message = eventArgs.Body;
                        Console.WriteLine(Encoding.UTF8.GetString(message));
                    };

                    channel.BasicConsume(queue: "message",
                        autoAck: true,
                        consumer: consumer);

                    Console.ReadKey();
                }
            }
        }

        private static IConnection GetConnection()
           => new ConnectionFactory()
           {
               HostName = "localhost"
           }
           .CreateConnection();
    }
}
