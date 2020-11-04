using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagerService.Integration.Interfaces;
using UserManagerService.Integration.Option;

namespace UserManagerService.Integration
{
    public class RecursiveExecutionService : IRecursiveService
    {
        private readonly ILogger<RecursiveExecutionService> _logger;
        private readonly ServiceOption _serviceOption;

        public RecursiveExecutionService(ILogger<RecursiveExecutionService> logger, ServiceOption serviceOption)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceOption = serviceOption ?? throw new ArgumentNullException(nameof(serviceOption));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var dateStart = DateTime.Now;
            await UpdateDataAsync(cancellationToken);
            // stop execution on cancel request
            if (cancellationToken.IsCancellationRequested) return;
            var expectedExecution = _serviceOption.RecursiveTimeoutMinutes;
            // wait if it required
            var dateEnd = DateTime.Now;
            var configuredDelayMinutes = expectedExecution;
            var configuredDelay = TimeSpan.FromMinutes(configuredDelayMinutes);
            var estimated = dateEnd - dateStart;
            // in сase if first task was executed too long
            if (estimated > configuredDelay)
            {
                await ExecuteAsync(cancellationToken);
                return;
            }
            var delay = configuredDelay - estimated;
            _logger.LogDebug($"Next execution after {delay}");
            // cancellation token will help us to stop task immediately when we waiting delay finish
            await Task.Delay(delay, cancellationToken);
            await ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Call userIfo
        /// </summary>
        private async Task UpdateDataAsync(CancellationToken cancellationToken)
        {
            //TODO 
            _logger.LogDebug($"Update executed at {DateTime.Now}");
        }
    }
}