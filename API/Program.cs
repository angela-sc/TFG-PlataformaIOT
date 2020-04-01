using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using CoAP;
using CoAP.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            //ICoapConfig coapConfig = new CoapConfig();
            //Console.WriteLine(coapConfig.DefaultBlockSize);
            CoapServer server = new CoapServer(5683);           
            server.Add(new RecursoPeticion());

            try
            {
                server.Start();

                Console.Write("CoAP server [{0}] is listening on", server.Config.Version);

                foreach (var item in server.EndPoints)
                {
                    Console.Write(" ");
                    Console.Write(item.LocalEndPoint);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    } 
}
