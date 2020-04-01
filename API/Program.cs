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
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            var logPath = config["LogPath"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(logPath)
                .CreateLogger();

            CoapServer server = new CoapServer(5683);           
            server.Add(new RecursoPeticion(Log.Logger));

            try
            {
                server.Start();

                Console.Write("CoAP server [{0}] is listening on", server.Config.Version);
                Log.Information("CoAP server [{0}] is listening on" + server.Config.Version);

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
                Console.WriteLine(ex.Message);
                Log.Fatal(ex.Message);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            Log.CloseAndFlush();
        }
    } 
}
