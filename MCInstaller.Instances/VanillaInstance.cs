using MCInstaller.Utilities;
using MCInstaller.Core.Exceptions;

namespace MCInstaller.Instances
{
    public class VanillaInstance : IServerInstance
    {
        public readonly JarReference Jar;
        public readonly string WorkingDir;

        public VanillaInstance(JarReference jar, string workingDir)
        {
            Jar = jar;
            WorkingDir = workingDir;
        }

        public Task Init()
        {
            throw new TodoException();
        }
    }
}
