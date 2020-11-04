using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Topshelf;
using UserManagerService.Integration.Interfaces;

namespace UserManagerService
{
    internal class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private const string СonfigFileName = "log4net.config";

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(СonfigFileName));

            ServiceProvider = Services.GetServices();
            InitWindowsService();
        }

        private static void InitWindowsService()
        {
            HostFactory.Run(c =>
            {
                c.UseLog4Net();
                c.OnException(OnException);
                c.Service<IWindowsService>(s =>
                {
                    s.ConstructUsing(ServiceProvider.GetService<IWindowsService>);
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
            });
        }

        private static void OnException(Exception ex) => Logger.Error(ex);
    }
}
