using MCInstaller.Core.Exceptions;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MCInstaller.Java
{
    public class JavaChecker
    {
        public JavaReference[] CheckForJava()
        {
            List<JavaReference> javaReferences = new();
            string[] javaPaths = GetJavaPaths();

            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = "-version",
            };

            foreach (var path in javaPaths)
            {
                using (var process = new Process())
                {
                    if (!Path.Exists(path))
                        continue;

                    startInfo.FileName = path;
                    process.StartInfo = startInfo;

                    try
                    {
                        process.Start();
                        if (process.WaitForExit(2000))
                        {
                            if (process.ExitCode != 0)
                                continue;
                            var output = process.StandardError;
                            string versionStr = output.ReadToEnd().Split('\n')[0].Split(' ')[2].Trim('"');
                            JavaVersion javaVer = JavaVersion.Parse(versionStr);
                            if (!javaReferences.Where(p => p.Version.FullVersion == javaVer.FullVersion).Any())
                                javaReferences.Add(new JavaReference(path, javaVer));
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
            return javaReferences.ToArray();
        }

        private string[] GetJavaPaths()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                List<string> javaPaths = new();

                var IsSymbolic = (string dir) =>
                {
                    return new DirectoryInfo(dir).LinkTarget != null;
                };

                var ScanDir = (string dir) =>
                {
                    if (!Directory.Exists(dir))
                        return;

                    foreach (var entry in Directory.GetDirectories(dir))
                    {
                        if (IsSymbolic(entry))
                            continue;

                        javaPaths.Add(Path.Combine(entry, "jre/bin/java"));
                        javaPaths.Add(Path.Combine(entry, "bin/java"));
                    }
                };

                // oracle RPMs
                ScanDir("/usr/java");
                // general locations used by distro packaging
                ScanDir("/usr/lib/jvm");
                ScanDir("/usr/lib64/jvm");
                ScanDir("/usr/lib32/jvm");
                // manually installed JDKs in /opt
                ScanDir("/opt/jdk");
                ScanDir("/opt/jdks");

                return javaPaths.ToArray();
            }
            throw new TodoException("Add support for different os.");
        }
    }
}
