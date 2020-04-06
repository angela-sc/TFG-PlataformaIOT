using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace EstacionBase.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfiguration();
            var logPath = config["LogPath"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(logPath)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;

            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                Log.Fatal(ex, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
              .AddJsonFile("appsettings-debug.json", optional: true, reloadOnChange: true)
#else
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
#endif
              .Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
