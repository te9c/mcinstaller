using MCInstaller.Core.Exceptions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

namespace MCInstaller.Java
{
    public class JavaChecker
    {
        public static JavaChecker Default { get; set; } = new();

        public JavaReference[] GetDefaultJavas()
        {
            List<JavaReference> javaReferences = new();
            string[] javaPaths = GetJavaPaths();

            foreach (var path in javaPaths)
            {
                JavaReference? javaRef;
                if (TryGetJavaReference(path, out javaRef!))
                {
                    if (!javaReferences.Where(p => p.Version.FullVersion == javaRef.Version.FullVersion).Any())
                        javaReferences.Add(javaRef);
                }
            }
            return javaReferences.ToArray();
        }

        public JavaReference GetJavaReference(string javaPath)
        {
            javaPath = Path.GetFullPath(javaPath);
            if (!Path.Exists(javaPath))
                throw new ArgumentException($"{javaPath} doesn't exists.", "javaPath");

            using (var process = new Process())
            {
                ProcessStartInfo startInfo = new()
                {
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    Arguments = "-version",
                    FileName = javaPath,
                };
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                    process.WaitForExit(2000);
                }
                catch (Win32Exception win)
                {
                    throw new ArgumentException($"Can't open {javaPath}.", "javaPath", win);
                }


                var output = process.StandardError.ReadToEnd();

                // java -version output example:
                //
                // openjdk version "1.8.0_372"
                // OpenJDK Runtime Environment (build 1.8.0_372-b07)
                // OpenJDK 64-Bit Server VM (build 25.372-b07, mixed mode)

                string versionStr = output.Split('\n')[0].Split(' ')[2].Trim('"');
                JavaVersion javaVer = JavaVersionParser.Default.Parse(versionStr);

                return new JavaReference(javaPath, javaVer);
            }
        }

        public bool TryGetJavaReference(string javaPath, out JavaReference? javaReference)
        {
            javaReference = null;
            try
            {
                javaReference = GetJavaReference(javaPath);
            }
            catch
            {
                return false;
            }
            return true;
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
