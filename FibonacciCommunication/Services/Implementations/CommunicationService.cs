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

        public CommunicationService(
            CalculationService calculationService,
            IMessageSender httpSender)
        {
            _calculationService = calculationService;
            _messageSender = httpSender;
        }

        public async Task ReceiveNumberAndSendNext(FibonacciNumber number)
        {
            var nextValue = _calculationService.CalculateNextByIndex(number.Index + 1);
            await _messageSender.SendMessage(nextValue);
        }
    }
}
