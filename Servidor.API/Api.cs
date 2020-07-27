using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using CoAP.Server;
using Libreria.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Servidor.API.Resources;

namespace Servidor.API
{
    public class Api : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private int puerto; //puerto de escucha de la API
        
        //private CoapClient cliente;
        private IServicioSeguridad servicioSeguridad;

        public Api(ILogger<Api> logger)
        {
            if (FactoriaServicios.Log == null)
                throw new ArgumentNullException("Log - {appsettings.json}");
            if(FactoriaServicios.Puerto == null)
                throw new ArgumentNullException("Puerto - {appsettings.json}");

            _logger = FactoriaServicios.Log;
            Int32.TryParse(FactoriaServicios.Puerto, out puerto);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //client = new CoapClient();
            //client.Uri = uri;

            //_logger.Information("API uri: " + uri.ToString());

            servicioSeguridad = FactoriaServicios.GetServicioSeguridad();

            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var config = CargaConfiguracion();
            //int puerto = config.GetValue<int>("Puerto");

            CoapServer server = new CoapServer(puerto);
            server.Add(new RecursoPeticion());

            try
            {
                server.Start();
                //Log.Information($"API (CoAP Server) {server.Config.Version} is listening on");

                //Console.Write("COAP server [{0}] is listening on", server.Config.Version);
                //Log.Debug($"COAP server {server.Config.Version} is listening on ");


                foreach (var item in server.EndPoints)
                {
                    //Console.Write(" ");
                    //Console.Write(item.LocalEndPoint);
                    //Log.Information($" {item.LocalEndPoint.ToString()}");
                    Log.Information($"API (CoAP Server) {server.Config.Version} is listening on {item.LocalEndPoint.ToString()}");
                }
                //Console.WriteLine();
            }
            catch (Exception ex)
            {
                Log.Fatal($"ERR API//SERVIDOR CoAP - {ex.Message}");
            }
            //Console.WriteLine("Press any key to exit.");
           // Console.ReadKey();

            Log.CloseAndFlush();

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("API running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        //private static IConfigurationRoot CargaConfiguracion()
        //{
        //    var config = new ConfigurationBuilder()
        //      .SetBasePath(Directory.GetCurrentDirectory())
        //      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //      .Build();

        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Debug()
        //        .Enrich.FromLogContext()
        //        .WriteTo.File(config["DirectorioLog"])
        //        .CreateLogger();

        //    FactoriaServicios.Log = Log.Logger;
        //    FactoriaServicios.FicheroClaveRSA = config["FicheroClaveRSA"];
        //    FactoriaServicios.CadenaConexion = config["CadenaConexion"];

        //    return config;
        //}
    }
}
