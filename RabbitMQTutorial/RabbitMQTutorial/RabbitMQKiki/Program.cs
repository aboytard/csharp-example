using RabbitMQ.Client;
using RabbitMQKiki;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // // if I want to : receive from a queue, after simulation of doing something acknowledge
            // Program_Part1_Part2.Tuto12();

            // receive from an exchange
            //Program_Part3.Tuto3(args);

            //TopicExchange.Tuto5(args);
            Rpc.Main(args).Wait();
        }
    }
}