using Commitex.Console;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", false, true)
    .Build();

var token = configuration.GetSection("Token").Value;
var predictor = new Commitex.Core.Predictor(token);
var prompt = "Propose a git commit message for this diff:\n\n" + new DiffSource().Source;
var prediction = await predictor.PredictAsync(prompt);

Console.WriteLine(prediction);