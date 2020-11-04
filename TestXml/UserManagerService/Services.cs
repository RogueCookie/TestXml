using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagerService.Integration;
using UserManagerService.Integration.Interfaces;
using UserManagerService.LoggerFactory;

namespace UserManagerService
{
    public class Services
    {
        public static IServiceProvider GetServices()
        {
            var services = new ServiceCollection();

            // add logger
            services.AddTransient(typeof(ILogger<>), typeof(Log4NetLogger<>));

            services
                .AddSingleton<IWindowsService, AppWindowsService>()
                .AddSingleton<IRecursiveService, RecursiveExecutionService>();

            var result = services.BuildServiceProvider();
            return result;
        }
    }
}