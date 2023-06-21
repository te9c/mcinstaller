using MCInstaller.Utilities;
using MCInstaller.Core.Exceptions;

namespace MCInstaller.Instances
{
    public class PaperInstance : IServerInstance
    {
        public JarReference Jar { get; init; }
        public JavaReference Java { get; init; }
        public string WorkingDir { get; init; }

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
