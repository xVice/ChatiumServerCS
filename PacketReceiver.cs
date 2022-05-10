
using System.Text.RegularExpressions;

namespace TheOneChatServer
{
    public static class PacketReceiver
    {
        public static List<Packet> packets = new List<Packet>();

        public static void VerifyPacket(Packet packet)
        {
            if (useRegex(packet.PacketString) == false)
            {
                packets.Add(packet);
                //string[] allSections = Regex.Split()
            }
            else
            {
                packet.PacketSender.SendMessageAsServer("Deine nachricht konnte nicht vom Server verarbeitet werden!");
            }
        }

        public static List<Packet> GetPacketQueue()
        {
            return packets;
        }

        public static bool useRegex(String input)
        {
            Regex regex = new Regex("<[a-zA-Z]+><[a-zA-Z]+><[a-zA-Z]+>", RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }
    }
}
