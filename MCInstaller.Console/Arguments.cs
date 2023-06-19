using CommandLine;
using CommandLine.Text;
using MCInstaller.Core;

namespace MCInstaller.Console
{
    public class Arguments
    {
        [Value(0, MetaName = "InstallationPath", Required = true, HelpText = "Sets installation path.")]
        public string InstallationPath { get; set; } = null!;

        [Option('v', "verbose", Required = false, HelpText = "Sets verbose message output. Mutually exclusive with quiet option.", SetName = "Verbose")]
        public bool Verbose { get; set; } = false;

        [Option('q', "quiet", Required = false, HelpText = "Do not print information messages. Mutually exclusive with verbose option.", SetName = "Quiet")]
        public bool Quiet { get; set; } = false;

        [Option('f', "forced", Required = false, HelpText = "Override empty directory restriction.")]
        public bool Forced { get; set; } = false;

        [Option('m', "minecraft-version", Required = true, HelpText = "Defines minecraft version to install.")]
        public string MinecraftVersion { get; set; } = null!;

        [Option('t', "type", Required = false, HelpText = "Defines type of server. Available options are Forge, Paper and Vanilla. Default are Vanilla.")]
        public ServerType ServerType { get; set; } = MCInstaller.Core.ServerType.Vanilla;

        [Usage(ApplicationAlias = "mcinstaller")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                    new Example("Install vanilla minecraft 1.12.2 server in current folder",
                        new Arguments { InstallationPath = ".", MinecraftVersion = "1.12.2" }),

                    new Example("Install forge minecraft 1.18.2 server in folder /home/Nikita/my-minecraft-server",
                        new Arguments { InstallationPath = "/home/Nikita/my-minecraft-server", MinecraftVersion = "1.18.2", ServerType = Core.ServerType.Forge})
                };
            }
        }
    }
}
