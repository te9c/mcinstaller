using MCInstaller.Core.Exceptions;

namespace MCInstaller.Utilities
{
    public class MinecraftVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;

        private MinecraftVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public override string ToString()
        {
            if (Patch == 0)
                return Major.ToString() + '.' + Minor.ToString();

            return Major.ToString() + '.' + Minor.ToString() + '.' + Patch.ToString();
        }

        public static MinecraftVersion Parse(string version)
        {
            string[] vers = version.Split('.');

            if (vers.Count() < 2 || vers.Count() > 3)
                throw new ParseException($"version {version} is in wrong format");

            int major = Int32.Parse(vers[0]);
            int minor = Int32.Parse(vers[1]);
            int patch = 0;
            if (vers.Count() == 3)
                patch = Int32.Parse(vers[2]);

            return new MinecraftVersion(major, minor, patch);
        }

        public static bool TryParse(string version, out MinecraftVersion? mcversion)
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
