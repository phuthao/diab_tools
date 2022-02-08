using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Worker
{
    public class Worker : BackgroundService
    {
        private const int MaxThreadPoolSize = 100;

        private const int DelayDequeue = 5;

        private readonly IJobQueue _jobQueue;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger _logger;

        private int _activeThreads;

        public Worker(IJobQueue jobQueue, IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _jobQueue = jobQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _activeThreads = 0;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.UtcNow}.");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Worker's running at: {DateTime.UtcNow}.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_activeThreads < MaxThreadPoolSize && !_jobQueue.IsEmpty())
                    {
                        var job = await _jobQueue.DequeueJobAsync(cancellationToken);

                        // Increase active threads
                        ++_activeThreads;

                        // Use for CPU-bound operations, not for I/O-bound operations
                        _ = Task.Run(() =>
                        {
                            _logger.LogInformation($"Processing new job at: {DateTime.UtcNow}.");

                            job(_serviceProvider).ContinueWith(task =>
                            {
                                --_activeThreads;

                                _logger.LogInformation($"Processed new job at: {DateTime.UtcNow}.");

                                if (task.IsFaulted)
                                {
                                    _logger.LogError(task.Exception.Message, task.Exception);
                                }
                            }, cancellationToken);
                        }, cancellationToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(DelayDequeue), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.UtcNow}.");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.UtcNow}.");

            base.Dispose();
        }
    }
}
