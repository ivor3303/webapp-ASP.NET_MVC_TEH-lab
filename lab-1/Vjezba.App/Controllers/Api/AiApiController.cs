using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Services;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/ai")]
public class AiApiController : ControllerBase
{
    private readonly AiService _aiService;

    public AiApiController(AiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("generate-maintenance")]
    public async Task<IActionResult> GenerateMaintenance([FromBody] AiGenerateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OpremaNaziv))
            return BadRequest("Naziv opreme je obavezan.");

        var result = await _aiService.GenerateMaintenanceDescription(request.OpremaNaziv, "održavanje");
        return Ok(new { text = result });
    }

    [HttpPost("generate-service-request")]
    public async Task<IActionResult> GenerateServiceRequest([FromBody] AiGenerateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OpremaNaziv))
            return BadRequest("Naziv opreme je obavezan.");

        var result = await _aiService.GenerateMaintenanceDescription(request.OpremaNaziv, "servisni zahtjev");
        return Ok(new { text = result });
    }
}

public class AiGenerateRequest
{
    public string OpremaNaziv { get; set; } = string.Empty;
}
