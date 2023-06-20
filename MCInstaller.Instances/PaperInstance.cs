using MCInstaller.Utilities;
using MCInstaller.Core.Exceptions;

namespace MCInstaller.Instances
{
    public class PaperInstance : IServerInstance
    {
        public readonly JarReference Jar;
        public readonly JavaReference Java;
        public readonly string WorkingDir;

        public PaperInstance(JarReference jar, JavaReference java, string workingDir)
        {
            Jar = jar;
            Java = java;
            WorkingDir = workingDir;
        }

        public Task Init()
        {
            throw new TodoException("Init method in PaperInstance");
        }
    }
}
