
using System.Text.RegularExpressions;

namespace TheOneChatServer
{ 
    public class Packet
    {
        public string Sender { get; set; }
        public int PacketType { get; set; }
        public string Text { get; set; }
        public string GUID { get; set; }
    }
}
