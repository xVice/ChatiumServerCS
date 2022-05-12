using System.Net.Sockets;
using Newtonsoft.Json;

namespace TheOneChatServer
{
    public class User
    {
        private Socket ClientSocket;
        private Thread userThread;
        private string UserName;
        private UserPool userPool;

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="Socket">The socket of the client</param>
        public User(Socket client, UserPool userPool, string user = "Anon")
        {
            this.userPool = userPool;
            ClientSocket = client;
            UserName = user;

            MakeUserThread();

            userPool.AddUser(this);
        }

        private void MakeUserThread()
        {
            Thread UserThread = new Thread(new ThreadStart(() => InitUser(this)));
            UserThread.Start();

            userThread = UserThread;
        }

        private void InitUser(User user)
        {
            string UserName = "Anon";
            bool isConnected = true;

            while (isConnected)
            {
                try
                {
                    //Thesis: this byte be to big mate, 1024 be filling the richtextbox big bad <- this was actually true, see msg.TakeWhile(x => x != 0).ToArray() 
                    //Deletes any /0's from the byte array!
                    byte[] packetByteArray = new byte[1024];
                    int packetSize = ClientSocket.Receive(packetByteArray);
                    string currentClientPacket = System.Text.Encoding.ASCII.GetString(packetByteArray.TakeWhile(x => x != 0).ToArray());
                    #pragma warning disable CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
                    Packet clientPacket = JsonConvert.DeserializeObject<Packet>(currentClientPacket);
                    clientPacket.Sender = UserName;
                    clientPacket.GUID = Guid.NewGuid().ToString();
                    PacketReceiver.Receive(this, clientPacket)

                    /*
                    if (curMessage.StartsWith(ChatCommands.GetPrefix()) && !curMessage.Equals(""))
                    {
                        ChatCommands.PerformCommand(user, curMessage);
                    }
                    else
                    {
                        userPool.SendToAllUsers(user, curMessage);
                    }
                    */
                }
                catch (System.Net.Sockets.SocketException socketExcept)
                {
                    //seems to throw a diffrent exception randomly,
                    //I dont really know why and i didnt happen again so i dont know whic exception it threw, only happens on disconnections so
                    //just catch the error and it should be fine
                    userPool.RemoveUser(this);
                    ClientSocket.Close();
                    userPool.SendToAllUsers(user, " disconnected from server!");
                    isConnected = false;
                }
            }
        }

        /// <summary>
        /// Sends a message to the user's socket
        /// </summary>
        /// <param name="message">The message in string format</param>
        public void SendMessage(string msg)
        {
            var craftedMassage = msg;
            ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(craftedMassage), 0, craftedMassage.Length, SocketFlags.None);
        }

        /// <summary>
        /// Sends a message to the user's socket as the server
        /// </summary>
        /// <param name="message">The message in string format</param>
        public void SendMessageAsServer(string msg)
        {
            var craftedMassage = "[Server]:" + msg;
            ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(craftedMassage), 0, craftedMassage.Length, SocketFlags.None);
        }

        /// <summary>
        /// Returns the username of the user object
        /// </summary>
        /// <returns>User.Username</returns>
        public string GetUsername()
        {
            return UserName;
        }
    }
}
