namespace MCInstaller.Java
{
    public class JavaVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Security;
        public readonly string? Prerelease;
        public readonly string FullVersion;

        internal JavaVersion(string fullVersion, int major, int minor, int security, string? prerelease = null)
        {
            Major = major;
            Minor = minor;
            Security = security;
            FullVersion = fullVersion;
            Prerelease = prerelease;
        }

        public override string ToString()
        {
            return FullVersion;
        }

        public bool IsPrerelease() => Prerelease != null;
    }
}
