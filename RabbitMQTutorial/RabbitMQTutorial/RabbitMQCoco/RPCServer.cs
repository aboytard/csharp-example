using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQCoco
{
    public static class RPCServer
    {
        public static void Tuto6(string[] args, IConnection connection, IModel channel)
        {
            while (channel.IsOpen)
            {
                channel.QueueDeclare(queue: "rpc_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                // the RPC Server is the consumer of the rabbitMQ communication in this case
                // we need to ask for the server to deal with a procedure
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                                        autoAck: false,
                                        consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                // here is how to add the consumer --> get better overview while creating serviceInterface on my own
                consumer.Received += (model, ea) =>
                {
                    string response = string.Empty;
                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        int n = int.Parse(message);
                        Console.WriteLine($" [.] Fib({message})");
                        response = Fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($" ERROR [.] {e.Message}");
                        response = string.Empty;
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "",
                                                routingKey: props.ReplyTo,
                                                basicProperties: replyProps,
                                                body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        // HERE THE CLOSING IS CURRENTLY NOT PROPERLY DONE
                        channel.Close();
                    }
                };
            };
        }

        static int Fib(int n)
        {
            if (n is 0 or 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
