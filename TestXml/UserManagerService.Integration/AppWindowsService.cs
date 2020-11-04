using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagerService.Integration.Interfaces;

namespace UserManagerService.Integration
{
    public class AppWindowsService : IWindowsService
    {
        private readonly ILogger<AppWindowsService> _logger;
        private readonly IRecursiveService _recursiveService;
        private CancellationTokenSource _cts;

        public AppWindowsService(ILogger<AppWindowsService> logger, IRecursiveService recursiveService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recursiveService = recursiveService ?? throw new ArgumentNullException(nameof(recursiveService));
        }

        /// <inheritdoc />
        public void Start()
        {
            _logger.LogDebug("Windows service started");

            _cts = new CancellationTokenSource();
            try
            {
                // unfortunately we can't use async with windows services,
                // but service still can be async when we will use it in another cases (console app, web app, etc)
                _recursiveService.ExecuteAsync(_cts.Token).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                // do nothing, this is expected behavior with "Task.Delay(..., cancellationToken)"
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            // request execution cancel
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _logger.LogInformation("Windows service stopping");
            }
            else _logger.LogInformation("Windows service not started");
        }
    }
}