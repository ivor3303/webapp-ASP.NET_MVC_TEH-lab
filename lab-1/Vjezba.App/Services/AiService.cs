using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Vjezba.App.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private const string AnthropicEndpoint = "https://api.anthropic.com/v1/messages";

    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateMaintenanceDescription(string opremaName, string type)
    {
        ConfigureAnthropicHeaders();

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

    public async Task<string> ParseOpremaFromText(string userText)
    {
        ConfigureAnthropicHeaders();

        var extractionPrompt = $"""
        Ti si pomoćnik za ekstrakciju podataka o radnoj opremi iz korisničkog teksta.
        Iz teksta izdvoji sljedeća polja: naziv, inventarniBroj, serijskiBroj, lokacijaId, proizvodacId, kategorijaOpremeId.

        Pravila odgovora:
        1) Vrati ISKLJUČIVO JSON objekt, bez dodatnog teksta.
        2) JSON mora imati točno ova polja: naziv, inventarniBroj, serijskiBroj, lokacijaId, proizvodacId, kategorijaOpremeId.
        3) Ako neko polje nije moguće zaključiti, vrati prazni string za tekstualna polja i 0 za ID polja.

        Korisnički unos:
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

        var response = await _httpClient.PostAsync(AnthropicEndpoint, content);
        if (!response.IsSuccessStatusCode)
        {
            return "{\"naziv\":\"\",\"inventarniBroj\":\"\",\"serijskiBroj\":\"\",\"lokacijaId\":0,\"proizvodacId\":0,\"kategorijaOpremeId\":0}";
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString()
            ?.Trim() ?? "{\"naziv\":\"\",\"inventarniBroj\":\"\",\"serijskiBroj\":\"\",\"lokacijaId\":0,\"proizvodacId\":0,\"kategorijaOpremeId\":0}";
    }

    private void ConfigureAnthropicHeaders()
    {
        var apiKey = _configuration["Anthropic:ApiKey"] ?? "placeholder";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }
}
