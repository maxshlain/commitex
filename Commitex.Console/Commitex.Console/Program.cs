using Commitex.Console;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", true, true)
    .Build();

var token = configuration.GetSection("Token").Value;

var url = "https://api.openai.com/v1/";
var prompt = "Propose a git commit message for this diff:\n\n" + new DiffSource().Source;
var output = await url
    .AppendPathSegment("completions")
    .WithHeader("Content-Type", "application/json")
    .WithOAuthBearerToken(token)
    .PostJsonAsync(new
    {
        model = "text-davinci-003",
        prompt = prompt,
        temperature = 0,
        max_tokens = 256,
        top_p = 1,
        frequency_penalty = 0,
        presence_penalty = 0
    })
    .ReceiveJson<Dictionary<string, object>>();

var choices = System.Text.Json.JsonSerializer.Deserialize(output["choices"].ToString(), typeof(List<Dictionary<string, object>>)) as List<Dictionary<string, object>>;
var x = output["choices"] as List<object>;
Console.WriteLine(output);