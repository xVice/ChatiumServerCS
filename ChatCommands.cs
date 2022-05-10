
namespace TheOneChatServer
{
    static class ChatCommands
    {
        private static string prefix = "/";
        public static void InitCommands()
        {
            ChatCommand helpCommand = new ChatCommand("/Help");
            ChatCommand pingCommand = new ChatCommand("/Ping");
            ChatCommand fickerCommand = new ChatCommand("/Ficker");
        }

        public static void PerformCommand(User sender, string command)
        {
            if (CommandManager.RunCommand(command))
            {
                sender.SendMessage(command);
                if (command.ToLower().Equals("/help"))
                {
                    foreach(ChatCommand chatCommand in CommandManager.GetCommands())
                    {
                        sender.SendMessage(chatCommand.GetCommandInput());
                    }
                }
                ChatCommand currentCommand = GetCommandByInputString(command);
                currentCommand.CallCommand();
            }
            else
            {
                sender.SendMessage(String.Format("Couldn't find the command: " + command));
            }
        }

        private static ChatCommand GetCommandByInputString(string cmdIn)
        {
            foreach(ChatCommand command in CommandManager.GetCommands())
            {
                if (command.GetCommandInput().Equals(cmdIn))
                {
                    return command;
                }
            }
            return null;
        }

        private static class CommandManager
        {
            private static List<ChatCommand> commands = new List<ChatCommand>();

            public static void AddCommand(string commandIn, ChatCommand command)
            {
                commands.Add(command);
            }

            public static bool RunCommand(string cmdIn)
            {
                foreach (ChatCommand chatCommand in commands)
                {
                    if (chatCommand.GetCommandInput().Equals(cmdIn, StringComparison.InvariantCultureIgnoreCase))
                    {
                        chatCommand.CallCommand();
                        return true;
                    }
                }
                return false;
            }

            public static List<ChatCommand> GetCommands()
            {
                return commands;
            }
        }

        private class ChatCommand
        {
            private Receiver commandReceiver;
            private string commandInput;
            public ChatCommand(string commandInput)
            {
                this.commandInput = commandInput;
                //commandReceiver = receiver;
                CommandManager.AddCommand(commandInput, this);
            }

            public void CallCommand()
            {
                //commandReceiver.CallCommand();
            }

            public string GetCommandInput()
            {
                return commandInput;
            }
            //Receiver commandReceiver;
        }

        public interface Receiver
        {
            public abstract void CallCommand();
        }

        public static string GetPrefix()
        {
            return prefix;
        }
    }
}
