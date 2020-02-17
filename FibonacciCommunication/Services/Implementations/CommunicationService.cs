using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models;
using System.Threading.Tasks;

namespace Services.Implementations
{
    /// <summary>
    /// Default implementation, inject needed parts
    /// </summary>
    public class CommunicationService : ICommunicationService
    {
        private readonly CalculationService _calculationService;
        private readonly IMessageSender _messageSender;
        private readonly ILogger<CommunicationService> _logger;

        public CommunicationService(
            CalculationService calculationService,
            IMessageSender httpSender,
            ILogger<CommunicationService> logger)
        {
            _calculationService = calculationService;
            _messageSender = httpSender;
            _logger = logger;
        }

        public async Task ReceiveNumberAndSendNext(FibonacciNumber number)
        {
            // delaying a bit, else the numbers are going too quickly
            await Task.Delay(1000);

            _logger.LogInformation("Received following number: ({index}, {value})", number.Index, number.Value);
            var nextValue = _calculationService.CalculateNextByIndex(number.Index + 1);

            _logger.LogInformation("Sending next number: ({index}, {value}) through: {senderType}", number.Index, number.Value, _messageSender.GetType().Name);
            await _messageSender.SendMessage(nextValue);
        }
    }
}
