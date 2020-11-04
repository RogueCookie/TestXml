using System;
using log4net;
using Microsoft.Extensions.Logging;

namespace UserManagerService.LoggerFactory
{
    public class Log4NetLogger<TType> : ILogger<TType>, IDisposable
    {
        private ILog _logger;

        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(typeof(TType));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
            // do nothing
        }

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //_output.WriteLine($"[{logLevel}] {formatter(state, exception)}");
            var message = formatter(state, exception);
            switch (logLevel)
            {
                case LogLevel.Debug: _logger.Debug(message); return;
                case LogLevel.Critical: _logger.Fatal(message, exception); return;
                case LogLevel.Error: _logger.Error(message, exception); return;
                case LogLevel.Information: _logger.Info(message); return;
                case LogLevel.None: _logger.Debug(message); return;
                //case LogLevel.Trace: _logger.Debug(message); return;
                case LogLevel.Warning: _logger.Warn(message); return;
            }
        }
    }
}