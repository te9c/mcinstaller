using CommandLine;
using MCInstaller.Jar;
using MCInstaller.Java;
using MCInstaller.Console;
using MCInstaller.Core;
using MCInstaller.Instances;
using ServerJarsAPI;
using System.Reflection;
using System.Runtime.InteropServices;

var parserResult = Parser.Default.ParseArguments<Arguments>(args);

return await parserResult.MapResult(async (opts) =>
    {
        Log.Verbose = opts.Verbose;
        Log.Quiet = opts.Quiet;

        Log.Information($"Starting {Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version}.");

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Log.Error($"Your OS is {RuntimeInformation.OSDescription}.");
            Log.Error("Only linux is supported at the moment.");
            return await Task.FromResult(-1);
        }

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

        if (opts.Forced)
        {
            Log.Warn("Running with --forced option. Be aware if any errors occurs!");
        }

        try
        {
            ServerJars serverJars = new();
            await serverJars.GetLatest("vanilla", "vanilla");
        }
        catch
        {
            Log.Error("Can't connect to ServerJarsAPI.");
            return await Task.FromResult(-1);
        }


        MinecraftVersion? mcversion;
        if (!MinecraftVersionParser.Default.TryParse(opts.MinecraftVersion, out mcversion!))
        {
            Log.Error($"version {opts.MinecraftVersion} is in wrong format.");
            return await Task.FromResult(-1);
        }


        JavaReference? java = null;
        if (opts.JavaPath != null)
        {
            Log.Warn("Manually specified java.");
            if (!Path.Exists(opts.JavaPath))
            {
                Log.Error($"Path {opts.JavaPath} doesn't exists.");
                return await Task.FromResult(-1);
            }
            if (!JavaChecker.Default.TryGetJavaReference(opts.JavaPath, out java))
            {
                Log.Error($"Java check of file {opts.JavaPath} failed.");
                Log.Error($"Please, ensure that you provided valid java path.");
                return await Task.FromResult(-1);
            }
        }

        IServerInstance instance = opts.ServerType switch
        {
            ServerType.Vanilla => new VanillaInstance(opts.InstallationPath, mcversion, java),
            // ServerType.Forge => new ForgeInstance(opts.InstallationPath, mcverision, java),
            // ServerType.Paper => new PaperInstance(opts.InstallationPath, mcversion, java),
            _ => throw new Exception()
        };

        return await instance.Init();
    },
    _ =>
    {
        return Task.FromResult(-1);
    });
