using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Vjezba.App.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateMaintenanceDescription(string opremaName, string type)
    {
        var apiKey = _configuration["Anthropic:ApiKey"] ?? "placeholder";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var requestBody = new
        {
            model = "claude-haiku-4-5-20251001",
            max_tokens = 300,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = $"Generiraj kratak opis {type} za opremu: {opremaName}. Opis treba biti profesionalan, na hrvatskom jeziku, maksimalno 2-3 rečenice."
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.anthropic.com/v1/messages", content);

        if (!response.IsSuccessStatusCode)
            return "Nije moguće generirati opis u ovom trenutku.";

        var responseJson = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? "Nije moguće generirati opis.";
    }
}
