using System.Security.Cryptography;
using Commitex.Console;
using Commitex.Core;
using Microsoft.Extensions.Configuration;

public class Settings
{
    public string Token { get; set; }
    public string Url { get; set; }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string diff;
        using (var reader = new StreamReader(Console.OpenStandardInput()))
        {
            diff = reader.ReadToEnd();
        }

        Console.WriteLine($"diff: {diff}");

        if (string.IsNullOrEmpty(diff) || diff.Length < 10)
        {
            Console.WriteLine("No diff provided");
            return;
        }

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();

        Settings settings = configuration.GetSection("Settings").Get<Settings>()
                            ?? throw new Exception("Settings not found");

        var prompt = "Propose a git commit message for this diff:\n\n" + diff;

        var predictor = new Commitex.Core.Predictor(settings.Token);

        var prediction = await predictor.PredictAsync(prompt);

        OsxClipboard.SetText(prediction);

        Console.WriteLine(prediction);
    }
}