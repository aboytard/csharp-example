using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace AspNetServiceLib.DataStructures
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ServiceMessage
    {
        private byte[] messageData;
        [JsonIgnore]
        public byte[] MessageData => messageData ?? (messageData = MessageEncoding.GetBytes(MessageText));

        private string messageText;
        [JsonIgnore]
        public string MessageText => messageText ?? (messageText = GetMessageText());

        private JObject messageJson;
        public JObject MessageJson => messageJson ?? (messageJson = JObject.Parse(MessageText));

        [JsonIgnore]
        public Encoding MessageEncoding { get; }

        [JsonIgnore]
        public string MimeType { get; }

        public ServiceMessage(byte[] message, string mimeType, Encoding encoding = null)
        {
            MessageEncoding = encoding ?? Encoding.UTF8;
            messageData = message ?? throw new ArgumentNullException();
            MimeType = mimeType;
        }

        public ServiceMessage(string message, Encoding encoding = null)
        {
            MessageEncoding = encoding ?? Encoding.UTF8;
            messageText = message ?? throw new ArgumentNullException();
            MimeType = "text/plain";
        }

        [JsonConstructor]
        public ServiceMessage(JObject messageJson, Encoding encoding = null)
        {
            MessageEncoding = encoding ?? Encoding.UTF8;
            this.messageJson = messageJson ?? throw new ArgumentNullException();
            MimeType = "application/json";
        }

        public override string ToString()
        {
            return MessageText;
        }

        private string GetMessageText()
        {
            if (messageData != null)
            {
                return MessageEncoding.GetString(messageData);
            }
            if (messageJson != null)
            {
                return messageJson.ToString();
            }
            return null;
        }
    }
}
