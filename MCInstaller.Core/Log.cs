namespace MCInstaller.Core
{
    public static class Log
    {
        public static bool Verbose = false;
        public static bool Quiet = false;

        private static void Write(string message, string tag, ConsoleColor foregroundColor)
        {
            var lines = message.Split('\n');
            foreach (var line in lines)
            {
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine($"[{tag}] -> " + line);
                Console.ResetColor();
            }
        }

        public static void Information(string message)
        {
            if (!Quiet)
            {
                Write(message, "INFO", ConsoleColor.White);
            }
        }

        public static void VerboseInformation(string message)
        {
            if (Verbose)
            {
                Write(message, "VERBOSE", ConsoleColor.Cyan);
            }
        }

        public static void Warn(string message)
        {
            Write(message, "WARN", ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            Write(message, "ERROR", ConsoleColor.Red);
        }

        public static void ExternalOutput(string message, string tag = "OUTPUT")
        {
            if (Verbose)
            {
                Write(message, tag, ConsoleColor.DarkGreen);
            }
        }
    }
}
