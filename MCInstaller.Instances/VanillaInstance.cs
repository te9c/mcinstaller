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
            WorkingDir = Path.GetFullPath(workingDir);
        }

        public async Task Init()
        {
            if (!Path.Exists(WorkingDir))
                throw new IOException($"Path {WorkingDir} doesn't exists");

            string pathToJar = Path.Combine(WorkingDir, Jar.GetFileName());
            if (!Path.Exists(pathToJar))
                await Jar.InstallAsync(WorkingDir);
        }
    }
}
