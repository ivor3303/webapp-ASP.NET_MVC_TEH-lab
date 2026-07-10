using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Vjezba.App.Services;

public class ParsedAiOprema
{
    public string Naziv { get; set; } = string.Empty;
    public string InventarniBroj { get; set; } = string.Empty;
    public string SerijskiBroj { get; set; } = string.Empty;
    public int LokacijaId { get; set; }
    public int ProizvodacId { get; set; }
    public int KategorijaOpremeId { get; set; }

    public static ParsedAiOprema Empty => new();
}

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiService> _logger;
    private const string AnthropicEndpoint = "https://api.anthropic.com/v1/messages";

    public AiService(HttpClient httpClient, IConfiguration configuration, ILogger<AiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public bool IsConfigured => ResolveApiKey() is not null;

    public async Task<string> GenerateMaintenanceDescription(string opremaName, string type)
    {
        var apiKey = ResolveApiKey();
        if (apiKey is null)
        {
            return "Nije moguće generirati opis jer Anthropic API ključ nije postavljen (vidi USER_SECRETS.md).";
        }

        ConfigureAnthropicHeaders(apiKey);

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

        var response = await _httpClient.PostAsync(AnthropicEndpoint, content);

        if (!response.IsSuccessStatusCode)
            return "Nije moguće generirati opis u ovom trenutku.";

        var responseJson = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? "Nije moguće generirati opis.";
    }

    public async Task<ParsedAiOprema> ParseOpremaFromText(string userText)
    {
        var apiKey = ResolveApiKey();
        if (apiKey is null)
        {
            _logger.LogWarning("AI parse preskočen: Anthropic:ApiKey nije postavljen (vidi USER_SECRETS.md).");
            return ParsedAiOprema.Empty;
        }

        ConfigureAnthropicHeaders(apiKey);

        var extractionPrompt = $"""
        Extract data from this text and return ONLY a JSON object with these exact fields: naziv, inventarniBroj, serijskiBroj.
        No other text, just JSON. If a field cannot be determined, use an empty string.

        Text:
        {userText}
        """;

        var requestBody = new
        {
            model = "claude-haiku-4-5-20251001",
            max_tokens = 300,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = extractionPrompt
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsync(AnthropicEndpoint, content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Poziv na Anthropic API nije uspio.");
            return ParsedAiOprema.Empty;
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Anthropic API je vratio status {StatusCode}. Odgovor: {Body}", response.StatusCode, responseBody);
            return ParsedAiOprema.Empty;
        }

        string rawText;
        try
        {
            var doc = JsonDocument.Parse(responseBody);
            rawText = doc.RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString()?.Trim() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Neuspjelo čitanje Anthropic odgovora. Sirovi odgovor: {Body}", responseBody);
            return ParsedAiOprema.Empty;
        }

        _logger.LogInformation("Sirovi AI odgovor za parsiranje opreme: {RawText}", rawText);

        return TryParseAiJson(rawText);
    }

    private ParsedAiOprema TryParseAiJson(string aiResponse)
    {
        try
        {
            var candidate = aiResponse.Trim();
            var firstBrace = candidate.IndexOf('{');
            var lastBrace = candidate.LastIndexOf('}');

            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                candidate = candidate.Substring(firstBrace, lastBrace - firstBrace + 1);
            }

            var parsed = JsonSerializer.Deserialize<ParsedAiOprema>(candidate, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return parsed ?? ParsedAiOprema.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Neuspjelo JSON parsiranje AI odgovora: {AiResponse}", aiResponse);
            return ParsedAiOprema.Empty;
        }
    }

    private string? ResolveApiKey()
    {
        var apiKey = _configuration["Anthropic:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "placeholder-api-key")
        {
            return null;
        }

        return apiKey;
    }

    private void ConfigureAnthropicHeaders(string apiKey)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }
}
