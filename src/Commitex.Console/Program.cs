using System.CommandLine;
using Serilog;
using Serilog.Events;

namespace Commitex.Console;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var verbose = new Option<bool>(
            name: "--verbose",
            description: "Verbose output",
            getDefaultValue: () => false);

        var root = new RootCommand("Commitex - let AI write better commit messages (better than you)");
        root.AddOption(verbose);
        
        root.SetHandler(
            (verboseArg) => MainImpl(verboseArg, args),
            verbose
        );
        
        await root.InvokeAsync(args);
    }

    private static async Task MainImpl(bool verbose, string[] args)
    {
        try
        {
            LogEventLevel consoleLevel = verbose ? LogEventLevel.Verbose : LogEventLevel.Information;
            LogEventLevel fileLevel = verbose ? LogEventLevel.Verbose : LogEventLevel.Debug;
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(consoleLevel)
                .WriteTo.File("log-.txt", fileLevel, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var diffProvider = new DiffProvider();
            var app = new ConsoleApp(diffProvider);
            await app.Main(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}