using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AcessoController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "IdadeMinima")]
    public IActionResult get()
    {

        return Ok("Acesso permitido!");
    }
}
