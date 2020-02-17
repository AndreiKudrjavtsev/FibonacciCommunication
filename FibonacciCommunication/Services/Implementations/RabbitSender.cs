using EasyNetQ;
using Services.Configuration;
using Services.Interfaces;
using Services.Models;
using System;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class RabbitSender : IMessageSender, IDisposable
    {
        private readonly IBus _bus;
        private readonly string _queueName;

        public RabbitSender(RabbitConfig config)
        {
            _queueName = config.QueueName;
            _bus = RabbitHutch.CreateBus(config.CreateConnectionString());
        }

        public async Task SendMessage(FibonacciNumber message)
        {
            await _bus.PublishAsync(message, configuration => configuration.WithQueueName(_queueName));
        }

        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}
