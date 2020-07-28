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
            servicioSeguridad = FactoriaServicios.GetServicioSeguridad();

            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         
            CoapServer server = new CoapServer(puerto);
            server.Add(new RecursoPeticion());

            try
            {
                server.Start();
              
                foreach (var item in server.EndPoints)
                {                    
                    Log.Information($"API (CoAP Server) {server.Config.Version} is listening on {item.LocalEndPoint.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal($"ERR API//SERVIDOR CoAP - {ex.Message}");
            }           
        }
    }
}
