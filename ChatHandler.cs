using System.Net;
using System.Net.Sockets;

namespace TheOneChatServer
{
    //TODO: Put Userpool init here, could prob run more then one server then cause of max modability, yes im going insane! <- works now :D
    internal class ChatHandler
    {
        private string ip;
        private int port;
        private Socket serverListener;
        private IPEndPoint serverEndPoint;
        private UserPool userPool;
        private Logger logger = new Logger("CHL");

        public ChatHandler(string ip, int port, UserPool userPool)
        {
            this.userPool = userPool;
            this.ip = ip;
            this.port = port;

            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            serverListener.Bind(serverEndPoint);
            serverListener.Listen();
            logger.Log("Server is Listening..");
            logger.Log("Running on " + ip + ":" + port);
            MakeJoinHandleThread();
        }

        private void MakeJoinHandleThread()
        {
            Thread UserThread = new Thread(new ThreadStart(() => HandleConnections()));
            UserThread.Start();
        }

        private void HandleConnections()
        {
            while (true)
            {
                Socket ClientSocket = default;
                ClientSocket = serverListener.Accept();
                User user = new User(ClientSocket, userPool);
                userPool.SendToAllUsersAsServer(user.GetUsername() + " Joined the server!");
            }
        }
    }
}
