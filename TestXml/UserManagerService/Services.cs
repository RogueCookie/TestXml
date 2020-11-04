using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagerService.Integration;
using UserManagerService.Integration.Interfaces;
using UserManagerService.Integration.Option;
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

            AddConfiguration(services);
            AddServiceOptions<ServiceOption>(services, "Service");

            var result = services.BuildServiceProvider();
            return result;
        }

        private static void AddConfiguration(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var result = builder.Build();
            services.AddSingleton<IConfiguration>(result);
        }

        /// <summary>
        /// Allow us to add additional custom options to the Service
        /// </summary>
        /// <typeparam name="TOptions">Name of the class which implement options</typeparam>
        /// <param name="services">Services</param>
        /// <param name="sectionName">Name of the section in appsettings file</param>
        /// <returns></returns>
        public static IServiceCollection AddServiceOptions<TOptions>(IServiceCollection services, string sectionName) where TOptions : class
            => services.AddSingleton<TOptions>(sp =>
            {
                var configurations = sp.GetRequiredService<IEnumerable<IConfiguration>>();
                foreach (var configuration in configurations)
                {
                    var section = configuration.GetSection(sectionName);
                    if (!section.Exists()) continue;

                    return section.Get<TOptions>();
                }

                return default;
            });
    }
}