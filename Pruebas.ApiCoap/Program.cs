using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoAP.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pruebas.ApiCoap.Resources;

namespace Pruebas.ApiCoap
{
    public class Program
    {
        public static void Main(String[] args)
        {
            CoapServer server = new CoapServer(5683);

            server.Add(new HelloWorldResource());

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
