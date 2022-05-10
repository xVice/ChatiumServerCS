
namespace TheOneChatServer
{
    public class UserPool
    {
        private List<User> users;
        private Logger logger = new Logger("USP");

        public UserPool()
        {
            users = new List<User>();
        }

        public void AddUser(User user)
        {
            users.Add(user);
            Console.Title = "Chatium server, currently " + GetUserCount() + " people are connected on: " + ChatServer.Program.ip + ":" + ChatServer.Program.port;
            logger.Log("Added " + user.GetUsername() + " to the UserPool!");
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
            Console.Title = "Chatium server, currently " + GetUserCount() + " people are connected on: " + ChatServer.Program.ip + ":" + ChatServer.Program.port;
            logger.Log("Removed " + user.GetUsername() + " from the UserPool!");
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public User GetUserByName(string name)
        {
            foreach(var user in users)
            {
                if (user.GetUsername().Equals(name))
                {
                    return user;
                }
            }
            return null;
        }

        public void SendToSpecificUser(User user, string message)
        {
            user.SendMessage(message);
        }

        public void SendToAllUsers(User sender, string message)
        {
            logger.Log("[" + sender.GetUsername() + "]said:" + message);
            foreach (User currentUser in GetUsers())
            {
                currentUser.SendMessage(message);
            }
        }

        public int GetUserCount()
        {
            return GetUsers().Count;
        }

        public void SendToAllUsersAsServer(string message)
        {
            logger.Log("[Server] said: " + message);
            foreach (User currentUser in GetUsers())
            {
                currentUser.SendMessage("[Server]:" + message);
            }
        }
    }
}
