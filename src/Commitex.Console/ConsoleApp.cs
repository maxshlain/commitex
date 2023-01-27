using Microsoft.Extensions.Configuration;

namespace Commitex.Console;

public class ConsoleApp
{
    public async Task Main(string[] args)
    {
        System.Console.WriteLine("Hello, World!");

        string diff;
        using (var reader = new StreamReader(System.Console.OpenStandardInput()))
        {
            diff = reader.ReadToEnd();
        }

        System.Console.WriteLine($"diff: {diff}");

        if (string.IsNullOrEmpty(diff) || diff.Length < 10)
        {
            System.Console.WriteLine("No diff provided");
            return;
        }

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();

        Settings settings = configuration.GetSection("Settings").Get<Settings>()
                            ?? throw new Exception("Settings not found");

        var prompt = "Propose a git commit message for this diff:\n\n" + diff;

        var predictor = new Core.Predictor(settings.Token);

        var prediction = await predictor.PredictAsync(prompt);

        OsxClipboard.SetText(prediction);

        System.Console.WriteLine(prediction);
    }
}