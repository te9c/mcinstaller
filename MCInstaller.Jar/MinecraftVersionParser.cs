using MCInstaller.Core.Exceptions;
using System.Text.RegularExpressions;

namespace MCInstaller.Jar
{
    public class MinecraftVersionParser
    {
        public MinecraftVersion Parse(string version)
        {
            int major = 1;
            int minor = 0;
            int patch = 0;

            var pattern = new Regex(@"^1{1}\.(?<minor>\d{1,2})(\.(?<patch>\d{1}))?$");

            if (pattern.IsMatch(version))
            {
                var match = pattern.Match(version);

                minor = Int32.Parse(match.Groups["minor"].Value);
                if (match.Groups["patch"].Success)
                {
                    patch = Int32.Parse(match.Groups["patch"].Value);
                }

                return new MinecraftVersion(major, minor, patch);
            }

            throw new ParseException($"Can't parse minecraft version: {version}.");
        }

        public bool TryParse(string version, out MinecraftVersion? mcversion)
        {
            try
            {
                mcversion = Parse(version);
            }
            catch
            {
                mcversion = null;
                return false;
            }

            return true;
        }
    }
}
