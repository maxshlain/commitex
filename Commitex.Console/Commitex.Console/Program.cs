using System;
using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .Build();

var token = configuration.GetSection("Token").Value;

var url = "https://api.openai.com/v1/";

var output = await url
    .AppendPathSegment("completions")
    .WithHeader("Content-Type", "application/json")
    .WithOAuthBearerToken(token)
    .PostJsonAsync(new
    {
        model = "code-davinci-002",
        prompt = "",
        temperature = 0,
        max_tokens = 256,
        top_p = 1,
        frequency_penalty = 0,
        presence_penalty = 0
    })
    .ReceiveJson<Dictionary<string, object>>();
    
Console.WriteLine(output);