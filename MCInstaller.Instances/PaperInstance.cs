using MCInstaller.Jar;
using MCInstaller.Java;

namespace MCInstaller.Instances
{
    public class PaperInstance : VanillaInstance, IServerInstance
    {
        public PaperInstance(string workingDir, MinecraftVersion version, JavaReference? java = null) : base(workingDir, version, java)
        {
        }
    }
}
