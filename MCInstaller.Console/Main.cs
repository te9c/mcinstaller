using CommandLine;
using MCInstaller.Utilities;
using MCInstaller.Console;
using MCInstaller.Core;
using System.Reflection;
using System.Diagnostics;

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
            Log.Error($"Directory {opts.InstallationPath} isn't empty. You can override it with --forced option.");
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

        var server = new Process();

        server.StartInfo.UseShellExecute = false;
        server.StartInfo.FileName = "java";
        server.StartInfo.CreateNoWindow = true;
        server.StartInfo.Arguments = "-jar " + Path.Combine(opts.InstallationPath, mcversion!.ToString() + '-' + opts.ServerType.ToString() + ".jar" + " nogui");
        server.StartInfo.WorkingDirectory = opts.InstallationPath;
        server.Start();

        server.WaitForExit();
        Log.Information(server.ExitCode.ToString());

        Log.Information("Initializing done.");
        Log.Information("You need to agree to eula and run file run.sh to complete installation");

        return await Task.FromResult(0);
    },
    _ =>
    {
        return Task.FromResult(-1);
    });
