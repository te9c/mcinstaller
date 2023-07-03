using System.Diagnostics;

namespace MCInstaller.Utilities
{
    public class JavaReference
    {
        public readonly string PathToJava;
        public readonly JavaVersion Version;
        public readonly string FullVersion;

        internal JavaReference(string path, JavaVersion version, string fullVersion)
        {
            PathToJava = path;
            Version = version;
            FullVersion = fullVersion;
        }
    }
}
