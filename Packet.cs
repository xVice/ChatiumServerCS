
namespace TheOneChatServer
{
    public class Packet
    {
        public string PacketString { get; }
        public User PacketSender { get; }
        private enum PacketType { PublicChatPacket, PrivateChatPacket, ChatCommandPacket};
        /// <summary>
        /// Creates a new chat Packet
        /// </summary>
        /// <param name="sender">The sender of the packet</param>
        /// <param name="inputPacketString">The string form of the packet</param>
        public Packet(User sender,string inputPacketString)
        {
            PacketString = inputPacketString;
            PacketSender = sender;
            PacketReceiver.VerifyPacket(this);
        }


    }
}
