using MCInstaller.Utilities;

namespace MCInstaller.Instances
{
    public interface IServerInstance
    {
        public JarReference Jar { get; init; }
        public JavaReference Java { get; init; }
        public string WorkingDir { get; init; }

        public Task Init();
    }
}
