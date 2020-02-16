using System.Threading.Tasks;
using DTO;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace InitiatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly HttpSenderService _httpSenderService;
        private readonly CalculationService _calculationService;
        private readonly IBus _bus;
        private readonly ILogger<FibonacciController> _logger;

        public FibonacciController(
            HttpSenderService httpSender,
            CalculationService calculationService,
            ConnectionConfiguration config,
            ILogger<FibonacciController> logger)
        {
            _httpSenderService = httpSender;
            _calculationService = calculationService;
            _logger = logger;
            _bus = RabbitHutch.CreateBus(config, register => { });

            var subscription = _bus.SubscribeAsync<RequestDto>("Fibonacci_calculation", OnMessageAsync, configure => configure.WithQueueName("Fibonacci_calculation"));


        }

        [HttpGet]
        [Route("initiate")]
        public async Task InitiateCalculation([FromQuery]int index)
        {
            _logger.LogInformation("Initiating fibonacci calculation; ");
            await _httpSenderService.SendNextRequest(new RequestDto { Index = index });
        }

        private async Task OnMessageAsync(RequestDto request)
        {
            _logger.LogInformation("Received request for calculation from Queue; \n" +
                "Given number index to calculate: {index}", request.Index);
            //var result = _calculationService.CalculateNext(request.Value, request.Index);
            //await _httpSenderService.SendNextRequest(new RequestDto { Value = result.Item1, Index = result.Item2 });

            var res = _calculationService.GetNext(new FibonacciNumber { Index = request.Index, Value = request.Value });
            await _httpSenderService.SendNextRequest(new RequestDto { Index = res.Index, Value = res.Value });
        }
    }
}
