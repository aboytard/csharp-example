using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQKiki
{
    public static class Program_Part1_Part2
    {
        public static void Tuto12()
        {
            const ushort MAX_CHANNELS = 1024 * 10;

            var factory = new ConnectionFactory { HostName = "localhost", DispatchConsumersAsync = true, RequestedChannelMax = MAX_CHANNELS };
            factory.UserName = "coco";
            factory.Password = "Coco1";

            // we are allowed to use the same factory publisher/consumer or two different factory
            //factory.UserName = "kiki";
            //factory.Password = "Kiki1";
            factory.Port = 5672; // using the default port there
            factory.VirtualHost = "/";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            // here I need to use an async event instead of the synchronous preconised one
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                Console.WriteLine(" [x] Done");

                // with this command, we check and acknoledge that the consumer actually received the n
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: "task_queue",
                                 autoAck: false, // here you can have the auto-acknoledge, that I disabled for the testing purpose
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
