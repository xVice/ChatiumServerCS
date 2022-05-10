using System.Net.Sockets;

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
                    byte[] msg = new byte[1024];
                    int size = ClientSocket.Receive(msg);
                    string curMessage = System.Text.Encoding.ASCII.GetString(msg.TakeWhile(x => x != 0).ToArray());
                    PacketReceiver.VerifyPacket(new Packet(this, curMessage));

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
                    //Cleanup after the user socket excepts/disconnects <- seems to throw a diffrent exception randomly,
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
