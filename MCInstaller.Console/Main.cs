using CommandLine;
using MCInstaller.Utilities;
using MCInstaller.Console;
using MCInstaller.Core;
using MCInstaller.Instances;
using System.Reflection;

var parserResult = Parser.Default.ParseArguments<Arguments>(args);

return await parserResult.MapResult(async (opts) =>
    {
        Log.Verbose = opts.Verbose;
        Log.Quiet = opts.Quiet;

        Log.Information("Starting mcinstaller v" + Assembly.GetExecutingAssembly().GetName().Version + '.');

        opts.InstallationPath = Path.GetFullPath(opts.InstallationPath);

        if (!Directory.Exists(opts.InstallationPath))
        {
            Log.Error($"Directory {opts.InstallationPath} doesn't exists.");
            return await Task.FromResult(-1);
        }

        if (Directory.GetFiles(opts.InstallationPath).Any() && !opts.Forced)
        {
            Log.Error($"Directory {opts.InstallationPath} isn't empty. You can override this restriction with --forced option.");
            return await Task.FromResult(-1);
        }

        MinecraftVersion? mcversion;
        if (!MinecraftVersion.TryParse(opts.MinecraftVersion, out mcversion))
        {
            Log.Error($"version {opts.MinecraftVersion} is in wrong format.");
            return await Task.FromResult(-1);
        }

        JarReference jar = new(mcversion!, opts.ServerType);

        await jar.InstallAsync(opts.InstallationPath);

        Log.Information("Initializing server.");

        IServerInstance server = opts.ServerType switch
        {
            ServerType.Vanilla => new VanillaInstance(),
            ServerType.Forge => new ForgeInstance(),
            ServerType.Paper => new PaperInstance(),
            _ => throw new Exception()
        };
        await server.InitAsync();

        Log.Information("Initializing done.");
        Log.Information("You need to agree with eula and run file run.sh to complete installation");

        return await Task.FromResult(0);
    },
    _ =>
    {
        return Task.FromResult(-1);
    });
