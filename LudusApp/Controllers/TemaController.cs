using LudusApp.Application.Services;
using LudusApp.Domain.TemaSettings;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemaController : BaseController<TemaService, Tema>
    {
        public TemaController(TemaService temaService) : base(temaService)
        {
        }

    }
}