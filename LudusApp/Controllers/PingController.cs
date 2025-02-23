using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Application.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(new { message = "Servidor ativo!", timestamp = DateTime.UtcNow });
    }
}
