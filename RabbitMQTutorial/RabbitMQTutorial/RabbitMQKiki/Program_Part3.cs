using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQKiki
{
    public static class Program_Part3
    {
        public static void Tuto3(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            factory.UserName = "kiki";
            factory.Password = "Kiki1";
            factory.Port = 5672; // using the default port there
            factory.VirtualHost = "/";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //// with fanout : to all
            //channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
            // with direct key
            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);


            // declare a server-named queue --> nothing in QueueDeclare use a random name 
            var queueName = channel.QueueDeclare().QueueName;

            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                                        Environment.GetCommandLineArgs()[0]);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }

            //channel.QueueBind(queue: queueName,
            //                  exchange: "logs",
            //                  routingKey: string.Empty);

            foreach (var severity in args)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "direct_logs",
                                  routingKey: severity);
            }

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine($" [x] {message}");
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
