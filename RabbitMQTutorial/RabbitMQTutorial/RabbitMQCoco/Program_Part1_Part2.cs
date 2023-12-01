using RabbitMQ.Client;
using System.Text;

namespace RabbitMQCoco
{
    public static class Program_Part1_Part2
    {
        public static void Tuto12(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost", DispatchConsumersAsync = true };
            factory.UserName = "coco";
            factory.Password = "Coco1";
            factory.Port = 5672; // using the default port there
            factory.VirtualHost = "/";

            var test = factory.ClientProperties;
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();


            // non durable queue ---> the acknoledgement wont work
            //channel.QueueDeclare(queue: "hello",
            //                     durable: false,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);

            // public to a queue instead of publishing to an exchange
            channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            // where is the args being defined..?
            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);

            // make the message persistant in the channel
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "task_queue",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
