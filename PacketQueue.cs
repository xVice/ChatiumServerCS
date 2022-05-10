using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheOneChatServer
{
    public class PacketQueue
    {

        public static void Init()
        {
            Thread PacketQueueThread = new Thread(new ThreadStart(() => HandlePackets()));
            PacketQueueThread.Start();
        }


        private static void ServePacket(Packet packet)
        {
            string[] packetSections = Regex.Split(packet.PacketString, "<[a-zA-Z]+><[a-zA-Z]+><[a-zA-Z]+>", RegexOptions.IgnoreCase);
            int packetID = int.Parse(packetSections[0]);
            string packetUserName = packet.PacketSender.GetUsername();
            string Text = packetSections[3];
            packet.PacketSender.SendMessage(Text);
            PacketReceiver.packets.Remove(packet);
        }

        private static void HandlePackets()
        {
            while (true)
            {
                foreach(Packet packet in PacketReceiver.GetPacketQueue())
                {
                    ServePacket(packet);
                }
            }
        } 
    }
}
