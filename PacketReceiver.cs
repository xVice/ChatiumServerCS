using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace TheOneChatServer
{
    public static class PacketReceiver
    {
        public static List<Packet> packetQueue = new List<Packet>();
        static StringBuilder sb = new StringBuilder();
        static StringWriter sw = new StringWriter(sb);

        public static void Receive(User packageSender, Packet packet)
        {
            if(JsonConvert.DeserializeObject<Packet>(packet) != null)
            {
                #pragma warning disable CS8604 // Mögliches Nullverweisargument.
                packetQueue.Add(JsonConvert.DeserializeObject<Packet>(packetjson));
            }
            else
            {

            }
            
        }

        
    }
}
