using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SimpleChatbot;

public sealed class OpenAiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAiClient(HttpClient httpClient, string apiKey, string model)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _model = model;
    }

    public async Task<string?> AskAsync(string userQuestion)
    {
        var payload = new
        {
            model = _model,
            messages = new object[]
            {
                new { role = "system", content = "You are a Unity game development assistant. Answer briefly with practical steps." },
                new { role = "user", content = userQuestion }
            },
            temperature = 0.2
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var resp = await _httpClient.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            return null;
        }

        await using var stream = await resp.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var text = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return text;
    }
}
