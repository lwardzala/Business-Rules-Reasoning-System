using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Reasoning.Host.Repositories;

namespace Reasoning.Host.Services
{
    public class ReasoningHostedService : BackgroundService
    {
        private readonly ILogger<ReasoningHostedService> _logger;
        public IBackgroundTaskQueue TaskQueueRepository { get; }

        public ReasoningHostedService(IBackgroundTaskQueue taskQueue, ILogger<ReasoningHostedService> logger)
        {
            TaskQueueRepository = taskQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Reasoning Background Service running");

            while (!stoppingToken.IsCancellationRequested)
            {
                await BackgroundProcessing(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reasoning Background Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueueRepository.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Couldn't process {nameof(workItem)}");
                }
            }
        }
    }
}
