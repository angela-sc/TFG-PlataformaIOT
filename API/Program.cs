using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using CoAP;
using CoAP.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace API
{
    public class Program
    {
        //    public static void Main(string[] args)
        //    {
        //        CreateHostBuilder(args).Build().Run();
        //    }

        //    public static IHostBuilder CreateHostBuilder(string[] args) =>
        //        Host.CreateDefaultBuilder(args)
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {
        //                webBuilder.UseStartup<Startup>();
        //            });
        //}

        //Servidor COAP que recibe las peticiones de la EB
        public static void Main(string[] args)
        {
            var config = CargaConfiguracion();
            int puerto = config.GetValue<int>("Puerto");

            CoapServer server = new CoapServer(puerto);    
            server.Add(new RecursoPeticion());

            try
            {
                server.Start();

                Console.Write("COAP server [{0}] is listening on", server.Config.Version);
                Log.Debug($"COAP server {server.Config.Version} is listening on ");

                foreach (var item in server.EndPoints)
                {
                    Console.Write(" ");
                    Console.Write(item.LocalEndPoint);
                    Log.Information(item.LocalEndPoint.ToString());
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            Log.CloseAndFlush();
        }

        private static IConfigurationRoot CargaConfiguracion()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(config["DirectorioLog"])
                .CreateLogger();

            FactoriaServicios.Log = Log.Logger;
            FactoriaServicios.FicheroClaveRSA = config["FicheroClaveRSA"];
            FactoriaServicios.CadenaConexion = config["CadenaConexion"];

            return config;
        }

        ////Metodo que devuelve un objeto IConfiguration para poder acceder a la informacion del fichero settings.json        
        //public static IConfigurationRoot GetConfiguration()
        //{
        //    return new ConfigurationBuilder()
        //      .SetBasePath(Directory.GetCurrentDirectory())
        //      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //      .Build();
        //}
    } 
}
