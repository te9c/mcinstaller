using CommandLine;
using MCInstaller.Jar;
using MCInstaller.Java;
using MCInstaller.Console;
using MCInstaller.Core;
using MCInstaller.Instances;
using System.Reflection;

var parserResult = Parser.Default.ParseArguments<Arguments>(args);

return await parserResult.MapResult(async (opts) =>
    {
        Log.Verbose = opts.Verbose;
        Log.Quiet = opts.Quiet;

        Log.Information($"Starting {Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version}.");

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

        MinecraftVersion? mcversion;
        if (!MinecraftVersion.TryParse(opts.MinecraftVersion, out mcversion!))
        {
            Log.Error($"version {opts.MinecraftVersion} is in wrong format.");
            return await Task.FromResult(-1);
        }

        if (opts.JavaPath != null)
        {
            Log.Warn("Manually specified java.");
            throw new MCInstaller.Core.Exceptions.TodoException();
        }
        Log.Information("Trying to find java...");
        JavaReference? java = Java.FindLatestOrDefault();
        if (java is null)
        {
            Log.Error("Cant find java.");
            Log.Error("Please, be sure that java is isntalled on your computer.");
            return await Task.FromResult(-1);
        }
        // if (opts.ServerType == ServerType.Forge)
        // {
        //     JavaVersion requiredVersion = mcversion.Minor switch
        //     {
        //         < 15 => throw new Exception(),
        //         _ => throw new Exception()
        //     };
        // }
        Log.Information("Java is found!");

        Log.VerboseInformation($"Java version: {java.FullVersion}");
        Log.VerboseInformation($"Java path: {java.PathToJava}");

        JarReference jar = new(mcversion, opts.ServerType);

        Log.Information("Initializing server.");

        IServerInstance server = opts.ServerType switch
        {
            ServerType.Vanilla => new VanillaInstance(jar, java, opts.InstallationPath),
            ServerType.Forge => new ForgeInstance(jar, java, opts.InstallationPath),
            ServerType.Paper => new PaperInstance(jar, java, opts.InstallationPath),
            _ => throw new Exception("Invalid ServerType")
        };
        await server.Init();

        Log.Information("Initializing done.");
        Log.Information($"Minecraft {jar.Type} {jar.Version} is installed.");
        Log.Information($"Now you can run run.sh to start your server.");

        return await Task.FromResult(0);
    },
    _ =>
    {
        return Task.FromResult(-1);
    });
