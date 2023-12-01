using RabbitMQ.Client;
using RabbitMQCoco;
using System.Data.Common;
using System.Threading.Channels;

namespace Application
{
    class Program
    {
        public static IConnection _connection { get; set; }
        public static IModel _channel { get; set; }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            factory.UserName = "coco";
            factory.Password = "Coco1";
            factory.Port = 5672; // using the default port there
            factory.VirtualHost = "/";

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            using (_connection)
            {
                using (_channel)
                {
                    RPCServer.Tuto6(args, _connection, _channel);
                };
            };

            // CHECK BUT UNUSED HERE 
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
        }

        // UNUSED HERE
        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        { 
            _channel.Dispose();
            _connection.Close();
        }
    }


}

