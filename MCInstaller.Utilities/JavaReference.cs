using System.Diagnostics;

namespace MCInstaller.Utilities
{
    public class JavaReference
    {
        public readonly string PathToJava;
        public readonly JavaVersion Version;

        private JavaReference(string path, JavaVersion version)
        {
            PathToJava = path;
            Version = version;
        }

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
                process.WaitForExit();

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
                    javaReferences.Add(new JavaReference(Path.Combine(line.ElementAt(2), "bin", "java"), version));
                }
            }
            return javaReferences.ToArray();
        }

        public static JavaReference? FindLatest()
        {
            var javaReferences = FindAll();

            return javaReferences.OrderBy(p => p.Version).LastOrDefault();
        }

        public static JavaReference? Find(JavaVersion version)
        {
            var javaReferences = FindAll();

            return javaReferences.FirstOrDefault(p => p.Version == version);
        }
    }
    public enum JavaVersion
    {
        v8,
        v11,
        v17
    }
}
