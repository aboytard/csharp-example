using Fleck;
using System.Collections.Concurrent;

namespace MultiThreadedEvent
{
    public class WsServer
    {
        private ConcurrentDictionary<IWebSocketConnection, WsClientHandler> _clients { get; set; }
        private WebSocketServer _simPlatformWSServer { get; set; }

        public delegate Task Event_A_DelagateHandler(string message);
        public event Event_A_DelagateHandler Event_A;

        public delegate Task Event_B_DelagateHandler(string message);
        public event Event_B_DelagateHandler Event_B;

        public delegate Task Event_C_DelagateHandler(string message);
        public event Event_C_DelagateHandler Event_C;

        public WsServer()
        {
            Console.WriteLine("WsServer Constructor");
            _simPlatformWSServer = new WebSocketServer("ws://" + "127.0.0.1" + ":" + "45678");
            _clients = new ConcurrentDictionary<IWebSocketConnection, WsClientHandler>();

            Event_A += WsServer_Event_A;
            Event_B += WsServer_Event_B; 
            Event_C += WsServer_Event_C;
            Console.WriteLine("WsServer Constructor initialized");
        }

        private Task WsServer_Event_C(string message)
        {
            Console.WriteLine(message);
            Thread.Sleep(200);
            Console.WriteLine("Event_C finished");
            return Task.CompletedTask;
        }

        private Task WsServer_Event_B(string message)
        {
            Console.WriteLine(message);
            Thread.Sleep(400);
            Console.WriteLine("Event_B finished");
            return Task.CompletedTask;
        }

        private Task WsServer_Event_A(string message)
        {
            Console.WriteLine(message);
            Thread.Sleep(200);
            Console.WriteLine("Event_A finished");
            return Task.CompletedTask;
        }

        public void Start()
        {
            _simPlatformWSServer.Start(socket =>
            {
                socket.OnOpen = () => OnSocketOpened(socket);
                socket.OnClose = () => OnSocketClosed(socket);
                socket.OnError = (ex) => OnSocketError(socket, ex);
                socket.OnMessage = async message =>
                {
                    await OnSocketMessage(message, socket);
                };
            });
        }

        private void OnSocketOpened(IWebSocketConnection socket)
        {
            Console.WriteLine("OnSocketOpened : " + $"Client on host {socket.ConnectionInfo.Host} connected.");
            _clients.TryAdd(socket, new WsClientHandler(socket));
        }

        private void OnSocketClosed(IWebSocketConnection socket)
        {
            Console.WriteLine("OnSocketClosed : " + $"Client on host {socket.ConnectionInfo.Host} disconnected.");
            _clients.TryRemove(socket, out var simPlatformWSClientHandler);
        }

        private void OnSocketError(IWebSocketConnection socket, Exception ex)
        {
            Console.WriteLine("OnSocketError : " + $"Error for socket: {socket.ConnectionInfo.Host} : {ex.Message}");
            OnSocketClosed(socket);
        }

        public async Task OnSocketMessage(string message, IWebSocketConnection socket)
        {
            if (message == "Event_A")
                await Event_A.Invoke(message);
            if (message == "Event_B")
                await Event_A.Invoke(message);
            if (message == "Event_C")
                await Event_A.Invoke(message);
        }
    }
}
