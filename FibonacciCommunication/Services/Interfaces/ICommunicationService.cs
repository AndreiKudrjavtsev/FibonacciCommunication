using Services.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommunicationService
    {
        Task ReceiveNumberAndSendNext(FibonacciNumber number);
    }
}
