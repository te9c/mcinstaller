using MCInstaller.Utilities;
using MCInstaller.Core.Exceptions;

namespace MCInstaller.Instances
{
    public class PaperInstance : IServerInstance
    {
        public readonly JarReference Jar;
        public readonly string WorkingDir;

        public PaperInstance(JarReference jar, string workingDir)
        {
            Jar = jar;
            WorkingDir = workingDir;
        }

        public Task Init()
        {
            throw new TodoException("Init method in PaperInstance");
        }
    }
}
