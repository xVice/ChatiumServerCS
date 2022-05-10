using TheOneChatServer;

namespace ChatServer
{
    class Program
    {
        public static string ip;
        public static int port;
        static void Main(string[] args)
        {
            Logger logger = new Logger("CTM");
            ip = args[0];
            port = int.Parse(args[1]);
            Console.Title = "Chatium server";
            logger.Log("Starting..");
            logger.Log("Inititliazing userpool..");
            UserPool userPool = new UserPool();
            //Init chatcommands
            logger.Log("Loading commands..");
            ChatCommands.InitCommands();
            //Init PacketQueue
            PacketQueue.Init();
            ChatHandler chatHandle = new ChatHandler(ip, port, userPool);

            Console.ReadKey();
        }
    }
}
