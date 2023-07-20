using MCInstaller.Java;
using MCInstaller.Jar;

namespace MCInstaller.Instances
{
    public interface IServerInstance
    {
        public JavaReference? Java { get; }
        public MinecraftVersion Version { get; init; }
        public string WorkingDir { get; init; }

        public Task<int> Init();
    }
}
