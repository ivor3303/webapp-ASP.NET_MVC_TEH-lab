using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddHttpClient();
builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.ConfigureHttpClient(client => client.BaseAddress = new Uri(VjezbaTools.BaseUrl))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<VjezbaTools>();

await builder.Build().RunAsync();

[McpServerToolType]
public class VjezbaTools
{
    private readonly HttpClient _httpClient;
    public const string BaseUrl = "https://localhost:7001";

    public VjezbaTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri(BaseUrl);
    }

    [McpServerTool, Description("Dohvati sve radne opreme iz sustava")]
    public Task<string> GetAllRadnaOprema() => GetJsonAsync("/api/radnaOprema");

    [McpServerTool, Description("Dohvati radnu opremu po ID-u")]
    public async Task<string> GetRadnaOpremaById([Description("ID radne opreme")] int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/radnaOprema/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return $"Oprema s ID {id} nije pronađena.";

            return await FormatResponseAsync(response);
        }
        catch (Exception ex)
        {
            return $"Greška: {ex.Message}";
        }
    }

    [McpServerTool, Description("Dohvati sve radnike iz sustava")]
    public Task<string> GetAllRadnici() => GetJsonAsync("/api/radnik");

    [McpServerTool, Description("Dohvati sve lokacije iz sustava")]
    public Task<string> GetAllLokacije() => GetJsonAsync("/api/lokacija");

    [McpServerTool, Description("Dohvati sva održavanja iz sustava")]
    public Task<string> GetAllOdrzavanja() => GetJsonAsync("/api/odrzavanje");

    [McpServerTool, Description("Pretraži radnu opremu po nazivu ili inventarnom broju")]
    public Task<string> SearchRadnaOprema([Description("Pojam za pretragu")] string query) =>
        GetJsonAsync($"/api/radnaOprema?q={Uri.EscapeDataString(query)}");

    private async Task<string> GetJsonAsync(string path)
    {
        try
        {
            var response = await _httpClient.GetAsync(path);
            return await FormatResponseAsync(response);
        }
        catch (Exception ex)
        {
            return $"Greška: {ex.Message}";
        }
    }

    private static async Task<string> FormatResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode
            ? content
            : $"Greška: HTTP {(int)response.StatusCode} - {content}";
    }
}
