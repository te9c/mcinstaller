namespace MCInstaller.Core
{
    public static class Log
    {
        public static bool Verbose = false;
        public static bool Quiet = false;

        public static void Information(string message)
        {
            if (!Quiet)
                Console.WriteLine("[INFO] -> " + message);
        }

        public static void VerboseInformation(string message)
        {
            if (!Quiet)
                if (Verbose)
                    Console.WriteLine("[INFO] -> " + message);
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] -> " + message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] -> " + message);
            Console.ResetColor();
        }
    }
}
