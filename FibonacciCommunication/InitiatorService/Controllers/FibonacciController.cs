using System.Linq;
using System.Threading.Tasks;
using InitiatorService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace InitiatorService.Controllers
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
        [Route("initiate")]
        public async Task InitiateCalculation([FromBody]InitiateRequest request)
        {
            _logger.LogInformation("Initiating {number} request with starting point: {@fib}", request.NumberOfCalculations, request.StartNumber);
            var initialTasks = Enumerable.Range(0, request.NumberOfCalculations)
                .Select(i => _communicationService.ReceiveNumberAndSendNext(request.StartNumber));

            await Task.WhenAll(initialTasks);
        }
    }
}
