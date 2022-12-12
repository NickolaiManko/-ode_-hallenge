using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindParkAPI.Daemon.Workers;

namespace WindParkAPI.Daemon.Jobs
{
    public class WindParkDataReader : IHostedService
    {
        private readonly ILogger<WindParkDataReader> _logger;
        private readonly IWorker _worker;

        public WindParkDataReader(ILogger<WindParkDataReader> logger, IWorker worker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _worker = worker ?? throw new ArgumentNullException(nameof(worker));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(WindParkDataReader)} is started at {DateTimeOffset.Now}");
            await _worker.DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(WindParkDataReader)} is stopped at {DateTimeOffset.Now}");

            return Task.CompletedTask;
        }
    }
}
