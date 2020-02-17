using Services.Models;

namespace InitiatorService.Dto
{
    public class InitiateRequest
    {
        public int NumberOfCalculations { get; set; }
        public FibonacciNumber StartNumber { get; set; }
    }
}
