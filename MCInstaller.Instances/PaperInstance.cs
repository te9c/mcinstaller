using MCInstaller.Utilities;

namespace MCInstaller.Instances
{
    public class PaperInstance : VanillaInstance, IServerInstance
    {
        public PaperInstance(JarReference jar, JavaReference java, string workingDir) : base(jar, java, workingDir)
        {
        }
    }
}
