using Microsoft.Extensions.Configuration;
using Serilog;

namespace Commitex.Console;

public class ConsoleApp
{
    private readonly DiffProvider _diffProvider;

    public ConsoleApp(DiffProvider diffProvider)
    {
        _diffProvider = diffProvider;
    }

    public async Task Main(string[] args)
    {
        var diff = _diffProvider.GetDiff();
        Log.Debug("Diff: {diff}", diff);
        if (string.IsNullOrEmpty(diff) || diff.Length < 10)
        {
            Log.Error("No diff provided");
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

        Log.Information(prediction);
    }
}