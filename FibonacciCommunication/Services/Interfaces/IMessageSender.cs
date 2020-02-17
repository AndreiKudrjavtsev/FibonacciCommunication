using Services.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessage(FibonacciNumber message);
    }
}
