using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Servidor.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CargarConfiguracion();
            var log = FactoriaServicios.Log;
            try
            {
                log.Information("Iniciando el servidor - API");
                CreateHostBuilder(args).Build().Run();
            }catch(Exception ex)
            {
                log.Fatal($"ERR PROGRAM (Servidor.API) - {ex.Message}");
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Api>();
                });

        private static void CargarConfiguracion()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
#else
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
#endif
                .Build();

            var directorioLog = config["DirectorioLog"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(directorioLog)
                .CreateLogger();

            FactoriaServicios.Log = Log.Logger;
            FactoriaServicios.CadenaConexion = config["CadenaConexion"];
            FactoriaServicios.FicheroClaveRSA = config["FicheroClaveRSA"];
            FactoriaServicios.Puerto = config["Puerto"];
        }
    }
}
