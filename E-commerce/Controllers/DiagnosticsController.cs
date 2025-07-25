using E_commerce.WebAPI.HostedServices;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly OrderProcessingService _processor;

        public DiagnosticsController(OrderProcessingService processor)
            => _processor = processor;
        [HttpPost("process-all")]
        public async Task<IActionResult> ProcessAll()
        {
            await _processor.ProcessAllAsync();
            return NoContent();
        }
    }
}
