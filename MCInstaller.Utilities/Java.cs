using System.Diagnostics;

namespace MCInstaller.Utilities
{
    public class Java
    {
        public static JavaReference[] FindAll()
        {
            List<JavaReference> javaReferences = new();
            using (var process = new Process())
            {
                var processInfo = new ProcessStartInfo
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "update-java-alternatives",
                    Arguments = "--list",
                };
                process.StartInfo = processInfo;
                process.Start();
                if (!process.WaitForExit(15000))
                {
                    process.Kill();
                    throw new Exception("process was killed");
                }

                if (process.ExitCode == 127)
                    throw new Exception("Cant find update-java-alternatives command");


                string[] output = process.StandardOutput.ReadToEnd().Split('\n');
                var tokensByLine = output.Where(p => p.Any()).Select((str) => str.Split(' ').Where(s => s.Any()));

                foreach (var line in tokensByLine)
                {
                    JavaVersion version;
                    if (line.ElementAt(0).Contains("java-1.8."))
                        version = JavaVersion.v8;
                    else if (line.ElementAt(0).Contains("java-1.11."))
                        version = JavaVersion.v11;
                    else if (line.ElementAt(0).Contains("java-1.17."))
                        version = JavaVersion.v17;
                    else
                        throw new Exception($"Unkown java version: {line.ElementAt(0)}");
                    javaReferences.Add(new JavaReference(Path.Combine(line.ElementAt(2), "bin", "java"), version, line.ElementAt(0)));
                }
            }
            return javaReferences.ToArray();
        }

        public static JavaReference? FindLatestOrDefault()
        {
            var javaReferences = FindAll();

            return javaReferences.OrderBy(p => p.Version).LastOrDefault();
        }

        public static JavaReference? FindOrDefault(JavaVersion version)
        {
            var javaReferences = FindAll();

            return javaReferences.FirstOrDefault(p => p.Version == version);
        }

        // public static JavaReference Parse(string pathToJava)
        // {
        //     if (!Path.Exists(pathToJava))
        //     {
        //         throw new IOException("${pathToJava} doesnt exists.");
        //     }
        //
        //     using (var process = new Process())
        //     {
        //         var processInfo = new ProcessStartInfo()
        //         {
        //             RedirectStandardError = true,
        //             RedirectStandardOutput = true,
        //             UseShellExecute = false,
        //             CreateNoWindow = true,
        //             FileName = pathToJava,
        //             Arguments = "-version",
        //         };
        //         process.StartInfo = processInfo;
        //         process.Start();
        //         process.WaitForExit();
        //
        //         string[] output = process.StandardOutput.ReadToEnd().Split('\n');
        //         string versionStr = output[0].Split(' ')[2].Trim('"');
        //         int major = Int32.Parse(versionStr.Split('.')[0]);
        //         JavaVersion javaVersion = major switch
        //         {
        //
        //             _ => throw new Exception("Unknown java version " + versionStr),
        //         };
        //     }
        // }
    }

    public enum JavaVersion
    {
        v8,
        v11,
        v17
    }
}
