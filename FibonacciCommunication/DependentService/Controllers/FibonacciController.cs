using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models;

namespace DependentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;
        private readonly ILogger<FibonacciController> _logger;

        public FibonacciController(
            ICommunicationService communicationService,
            ILogger<FibonacciController> logger)
        {
            _communicationService = communicationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("next")]
        public async Task CalculateNext([FromBody]FibonacciNumber number)
        {
            _logger.LogInformation("Received following number: @number", number);
            await _communicationService.ReceiveNumberAndSendNext(number);
        }
    }
}
