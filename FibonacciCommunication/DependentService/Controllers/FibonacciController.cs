using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;

namespace InitiatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly CalculationService _calculationService;
        private readonly IBus _bus;
        private readonly ILogger<FibonacciController> _logger;
        

        public FibonacciController(
            CalculationService calculationService,
            ConnectionConfiguration config,
            ILogger<FibonacciController> logger)
        {
            _calculationService = calculationService;
            _logger = logger;
            _bus = RabbitHutch.CreateBus(config, register => { });
        }

        [HttpPost]
        [Route("next")]
        public async Task CalculateNext([FromBody]RequestDto request)
        {
            _logger.LogInformation("Received request for calculation from Queue; \n" +
               "Given number index to calculate: {index}", request.Index);

            await Task.Delay(2000);

            //var result = _calculationService.CalculateNext(request.Value, request.Index);
            //await _bus.PublishAsync<RequestDto>(new RequestDto { Value = result.Item1, Index = result.Item2 });

            var res = _calculationService.GetNext(new FibonacciNumber { Index = request.Index, Value = request.Value });
            await _bus.PublishAsync<RequestDto>(new RequestDto { Index = res.Index, Value = res.Value });
        }
    }
}
