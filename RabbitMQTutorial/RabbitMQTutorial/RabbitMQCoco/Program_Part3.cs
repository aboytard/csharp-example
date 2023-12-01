using System.Text;
using RabbitMQ.Client;

namespace RabbitMQCoco
{
    public static class Program_Part3
    {
        public static void Tuto3(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            factory.UserName = "coco";
            factory.Password = "Coco1";
            factory.Port = 5672; // using the default port there
            factory.VirtualHost = "/";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //// fanout: broadcast the message to any queue the channel knows
            //channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            // direct exchange
            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);


            var severity = (args.Length > 0) ? args[0] : "info";
            var message = (args.Length > 1)
              ? string.Join(" ", args.Skip(1).ToArray())
              : "Hello World!";

            //var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);
            //channel.BasicPublish(exchange: "logs",
            //                     routingKey: string.Empty,
            //                     basicProperties: null,
            //                     body: body);
            channel.BasicPublish(exchange: "direct_logs",
                     routingKey: severity,
                     basicProperties: null,
                     body: body);
            //Console.WriteLine($" [x] Sent {message}");
            Console.WriteLine($" [x] Sent '{severity}':'{message}'");


            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
