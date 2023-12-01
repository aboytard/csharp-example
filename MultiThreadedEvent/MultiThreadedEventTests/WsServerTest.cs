using Fleck;
using Moq;
using MultiThreadedEvent;
using NUnit.Framework;
using System.Net.WebSockets;
using System.Text;

namespace MultiThreadedEventTests
{
    public class WsServerTest
    {
        private WsServer _server;
        private ClientWebSocket _clientSocket;

        [SetUp]
        public void Setup()
        {
            _server = new WsServer();
            _server.Start();

            _clientSocket = new ClientWebSocket();
            _clientSocket.ConnectAsync(new Uri("ws://127.0.0.1:45678"), CancellationToken.None).Wait();
        }

        [Test]
        public async Task MultiEventThreadedTest()
        {
            await SendMessage("Event_A");
            await SendMessage("Event_B");
            await SendMessage("Event_C");
            await Task.Delay(1000);
        }

        [TearDown]
        public void Teardown()
        {
            _clientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
            _clientSocket.Dispose();
        }

        private async Task SendMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await _clientSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine(message + "SENT");
        }
    }
}