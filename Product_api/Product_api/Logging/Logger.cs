using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Product_api.Logging
{

    public class Logger: ILogger
    {
        private readonly Serilog.ILogger _logger;

        public Logger()
        {
            var logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "/logs"); // Specify the folder path
            Directory.CreateDirectory(logFolderPath); // Ensure the folder exists

            _logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolderPath, $"{DateTime.Now:yyyyMMdd}-products.log"), rollingInterval: RollingInterval.Day) // Specify the log file path
                .CreateLogger();

        }

        public void Log(string message)
        {
            _logger.Information(message);
        }

    }
}