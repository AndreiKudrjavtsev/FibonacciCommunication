using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Services.Configuration;
using Services.Interfaces;
using Services.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InitiatorService
{
    public class WorkerService : IHostedService, IDisposable
    {
        private readonly IBus _bus;
        private ISubscriptionResult _subscription;
        private readonly string _queueName;
        private readonly ICommunicationService _communicationService;

        public WorkerService(
            RabbitConfig rabbitConfig,
            ICommunicationService communicationService)
        {
            _bus = RabbitHutch.CreateBus(rabbitConfig.CreateConnectionString());
            _queueName = rabbitConfig.QueueName;
            _communicationService = communicationService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = _bus.SubscribeAsync<FibonacciNumber>("Fibonacci_calculation", 
                _communicationService.ReceiveNumberAndSendNext,
                configure => configure.WithQueueName(_queueName));

            return Task.CompletedTask;
        }

        public void StartListen()
        {
            _subscription = _bus.SubscribeAsync<FibonacciNumber>("Fibonacci_calculation",
                _communicationService.ReceiveNumberAndSendNext,
                configure => configure.WithQueueName(_queueName));
        }

        // Not exactly dispose pattern, but will do
        public void Dispose()
        {
            _subscription?.Dispose();
            _bus?.Dispose();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
