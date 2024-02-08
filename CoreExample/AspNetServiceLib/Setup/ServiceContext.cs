using RabbitMQ.Client;

namespace AspNetServiceLib.Setup
{
    public class ServiceContext : IDisposable
    {
        private const ushort MAX_CHANNELS = 1024 * 10;

        public string ServiceName { get; }

        public IConnection Connection { get; }

        /// <summary>
        /// Initializes the context with a RabbitMQ connection object.
        /// It is important that the dispatcher in the connection is async.
        /// The service context will own the connection, meaning that it will dispose it.
        /// </summary>
        /// <param name="connection">RabbitMQ connection object</param>
        public ServiceContext(string serviceName, IConnection connection)
        {
            ServiceName = serviceName;
            Connection = connection;
        }

        public ServiceContext(string serviceName, string hostname)
        {
            ServiceName = serviceName;
            var factory = new ConnectionFactory() { HostName = hostname, DispatchConsumersAsync = true, RequestedChannelMax = MAX_CHANNELS };
            Connection = factory.CreateConnection();
        }

        public ServiceContext(string serviceName, Uri uri)
        {
            ServiceName = serviceName;
            var factory = new ConnectionFactory() { Uri = uri, DispatchConsumersAsync = true, RequestedChannelMax = MAX_CHANNELS };
            Connection = factory.CreateConnection();
        }

        /// <summary>
        /// Constructor for unit testing purposes to allow not setting a connection.
        /// Service will then not have any connection to a broker.
        /// </summary>
        /// <param name="serviceName"></param>
        public ServiceContext(string serviceName)
        {
            ServiceName = serviceName;
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
