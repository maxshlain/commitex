using System.Text.Json;
using Flurl;
using Flurl.Http;

namespace Commitex.Core;

public class Predictor
{
    private readonly string _token;

    public Predictor(string token)
    {
        _token = token;
    }

    public async Task<string> PredictAsync(string text)
    {
        var url = "https://api.openai.com/v1/";

        var output = await url
            .AppendPathSegment("completions")
            .WithHeader("Content-Type", "application/json")
            .WithOAuthBearerToken(_token)
            .PostJsonAsync(new
            {
                model = "text-davinci-003",
                prompt = text,
                temperature = 0,
                max_tokens = 256,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            })
            .ReceiveJson<Dictionary<string, object>>();

        string choicesText = output["choices"].ToString() ?? throw new NullReferenceException();
        List<Dictionary<string, object>> choices = JsonSerializer
                                                       .Deserialize<List<Dictionary<string, object>>>(choicesText)
                                                   ?? throw new NullReferenceException("Failed to deserialize choices");

        var firstChoice = choices[0]["text"].ToString()
                          ?? throw new NullReferenceException("Failed to get first choice");

        firstChoice = firstChoice.Trim().Trim('"');

        return firstChoice;
    }
}