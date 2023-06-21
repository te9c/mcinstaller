using MCInstaller.Core;
using ServerJarsAPI;

namespace MCInstaller.Utilities
{
    public class JarReference
    {
        public readonly MinecraftVersion Version;
        public readonly ServerType Type;
        public readonly string TypeString;
        public readonly string CategoryString;

        public JarReference(MinecraftVersion version, ServerType type)
        {
            Version = version;
            Type = type;

            TypeString = type switch
            {
                ServerType.Vanilla => "vanilla",
                ServerType.Forge => "modded",
                ServerType.Paper => "servers",
                _ => throw new Exception()
            };
            CategoryString = type switch
            {
                ServerType.Vanilla => "vanilla",
                ServerType.Forge => "forge",
                ServerType.Paper => "paper",
                _ => throw new Exception()
            };
        }

        public async Task<bool> IsValidAsync()
        {
            var serverJars = new ServerJars();

            try
            {
                await serverJars.GetDetails(TypeString, CategoryString, Version.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetFileName()
        {
            return Version.ToString() + '-' + Type.ToString() + ".jar";
        }

        public async Task InstallAsync(string path)
        {
            if (!await IsValidAsync())
                throw new Exception($"{this} is not valid");

            Log.Information($"Downloading {Type.ToString()} {Version.ToString()}.");
            Log.VerboseInformation($"Path: {Path.GetFullPath(path)}");

            var serverJar = new ServerJars();

            using (var fileStream = File.Create(Path.Combine(path, GetFileName())))
            {
                await serverJar.GetJar(fileStream, TypeString, CategoryString, Version.ToString());
                await fileStream.FlushAsync();
                Log.VerboseInformation($"Downloaded {fileStream.Length / 1024 / 1024}MB to {fileStream.Name}");
            }
            Log.Information($"Jar downloaded.");
        }
    }
}
