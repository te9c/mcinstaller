using CommandLine;
using MCInstaller.Jar;
using MCInstaller.Java;
using MCInstaller.Console;
using MCInstaller.Core;
using MCInstaller.Instances;
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

        MinecraftVersion? mcversion;
        if (!MinecraftVersionParser.Default.TryParse(opts.MinecraftVersion, out mcversion!))
        {
            Log.Error($"version {opts.MinecraftVersion} is in wrong format.");
            return await Task.FromResult(-1);
        }

        JavaReference? java;
        if (opts.JavaPath != null)
        {
            Log.Warn("Manually specified java.");
            JavaChecker.Default.TryGetJavaReference(opts.JavaPath, out java);

            if (java is null)
            {
                Log.Error($"Invalid {opts.JavaPath}.");
                Log.Error($"Please, ensure that you provide right path to java.");
                return await Task.FromResult(-1);
            }
        }
        else
        {

            Log.Information("Trying to find java...");

            java = JavaChecker.Default.GetDefaultJavas().OrderBy(p => p.Version.Major).LastOrDefault();
            if (java is null)
            {
                Log.Error("Can't find java.");
                Log.Error("Please, be sure that java is isntalled on your computer.");
                return await Task.FromResult(-1);
            }

            Log.Information("Java is found!");
        }

        Log.VerboseInformation($"Java version: {java.Version.FullVersion}");
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
