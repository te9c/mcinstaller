using MCInstaller.Core;
using ServerJarsAPI;
using ServerJarsAPI.Events;

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

        public async Task InstallAsync(string path)
        {
            if (!await IsValidAsync())
                throw new Exception($"{this} is not valid");

            Log.Information($"Downloading {Type.ToString()} {Version.ToString()} in {Path.GetFullPath(path)}.");

            var serverJar = new ServerJars();

            using var fileStream = File.Create(Path.Combine(path, Version.ToString() + '-' + Type.ToString() + ".jar"));
            Progress<ProgressEventArgs> progress = new();
            progress.ProgressChanged += (_, e) =>
            {
                if (!Log.Quiet)
                    Console.Write($"\r[INFO] -> Progress: {e.ProgressPercentage}% ({e.BytesTransferred / 1024 / 1024}MB / {e.TotalBytes / 1024 / 1024}MB)          ");
            };
            await serverJar.GetJar(fileStream, TypeString, CategoryString, Version.ToString(), progress: progress);
            await fileStream.FlushAsync();
            Console.WriteLine();

            Log.Information($"Jar downloaded.");
        }
    }
}
