using MCInstaller.Core.Exceptions;
using System.Text.RegularExpressions;

namespace MCInstaller.Java
{
    public class JavaVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Security;
        public readonly string? Prerelease;
        public readonly string FullVersion;

        private JavaVersion(string fullVersion, int major, int minor, int security, string? prerelease = null)
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

        public static JavaVersion Parse(string version)
        {
            int major = 0;
            int minor = 0;
            int security = 0;
            string? prerelease = null;
            Regex pattern;

            if (version.StartsWith("1."))
            {
                pattern = new Regex(@"1[.](?<major>[0-9]+)([.](?<minor>[0-9]+))?(_(?<security>[0-9]+)?)?(-(?<prerelease>[a-zA-Z0-9]+))?");
            }
            else
            {
                pattern = new Regex(@"(?<major>[0-9]+)([.](?<minor>[0-9]+))?([.](?<security>[0-9]+))?(-(?<prerelease>[a-zA-Z0-9]+))?");
            }

            if (pattern.IsMatch(version))
            {
                var match = pattern.Match(version);
                major = Int32.Parse(match.Groups["major"].Value);

                if (match.Groups["minor"].Success)
                    minor = Int32.Parse(match.Groups["minor"].Value);

                if (match.Groups["security"].Success)
                    security = Int32.Parse(match.Groups["security"].Value);

                if (match.Groups["prerelease"].Success)
                    prerelease = match.Groups["prerelease"].Value;

                return new JavaVersion(version, major, minor, security, prerelease);
            }

            throw new ParseException($"Can't parse java version: {version}.");
        }

        public static bool TryParse(string version, out JavaVersion? java)
        {
            try
            {
                java = Parse(version);
            }
            catch
            {
                java = null;
                return false;
            }

            return true;
        }
    }
}
