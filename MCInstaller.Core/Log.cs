namespace MCInstaller.Core
{
    public static class Log
    {
        public static bool Verbose = false;
        public static bool Quiet = false;

        public static void Information(string message)
        {
            if (!Quiet)
            {
                var lines = message.Split('\n');
                foreach (var line in lines)
                {
                    Console.WriteLine("[INFO] -> " + line);
                }
            }
        }

        public static void VerboseInformation(string message)
        {
            if (!Quiet)
            {
                if (Verbose)
                {
                    var lines = message.Split('\n');
                    foreach (var line in lines)
                    {
                        Console.WriteLine("[INFO] -> " + line);
                    }
                }
            }
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var lines = message.Split('\n');
            foreach (var line in lines)
            {
                Console.WriteLine("[WARN] -> " + line);
            }
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var lines = message.Split('\n');
            foreach (var line in lines)
            {
                Console.WriteLine("[ERROR] -> " + line);
            }
            Console.ResetColor();
        }
    }
}
