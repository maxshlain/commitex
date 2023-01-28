using Serilog;

namespace Commitex.Console;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
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