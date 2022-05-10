
namespace TheOneChatServer
{
    internal class Logger
    {
        private string logTag;
        private ConsoleColor logColor;
        public Logger(string logTag = "DEB", ConsoleColor loggerColor = ConsoleColor.Gray)
        {
            this.logColor = loggerColor;
            this.logTag = logTag;
        }

        public void Log(string log)
        {
            Console.ForegroundColor = this.logColor;
            Console.WriteLine(BuildLog(log));
            Console.ResetColor();
        }

        private string BuildLog(string msg)
        {
            var timeString = DateTime.Now.ToString("MM/dd/yyyy");
            return "[" + timeString + "]-[" + logTag + "]:" + msg; 
        }
    }
}
