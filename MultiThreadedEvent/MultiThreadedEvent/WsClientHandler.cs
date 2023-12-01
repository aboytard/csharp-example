using Fleck;

namespace MultiThreadedEvent
{
    public class WsClientHandler
    {
        private readonly IWebSocketConnection socket;

        public WsClientHandler(IWebSocketConnection socket)
        {
            this.socket = socket;
        }

        public async Task Send(string msg)
        {
            try
            {
                await socket.Send(msg);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send message to client [{socket.ConnectionInfo.Host}]: {ex.Message}");
            }
        }
    }
}
