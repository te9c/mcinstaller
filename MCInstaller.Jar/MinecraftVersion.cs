namespace MCInstaller.Jar
{
    public class MinecraftVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;

        internal MinecraftVersion(int major, int minor, int patch)
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
    }
}
