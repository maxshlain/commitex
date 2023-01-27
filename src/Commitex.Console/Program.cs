using Serilog;

namespace Commitex.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        Log.Logger = log;
        Log.Information("The global logger has been configured");
        
        try
        {
            await new ConsoleApp().Main(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}