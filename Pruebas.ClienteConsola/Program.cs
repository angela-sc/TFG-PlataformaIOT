using CoAP;
using Libreria.Entidades;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Pruebas.ClienteConsola
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //await TestGet_ApiNormal();
            //await TestGet_ApiCoap();
            await TestPost_ApiCoap();
        }

        private static async Task TestGet_ApiNormal()
        {
            var respuesta = await ClientHelper.GetAsync();

            Console.WriteLine(respuesta);
        }

        private static async Task TestGet_ApiCoap()
        {
            var client = new CoapClient();

            client.Uri = new Uri("coap://localhost:5683/helloworld");
            var res = client.Get();

            Console.WriteLine(res.ResponseText);
        }

        private static async Task TestPost_ApiCoap()
        {
            var client = new CoapClient();
            client.Uri = new Uri("coap://localhost:5683/helloworld");

            var json = JsonConvert.SerializeObject(new EntidadSensor()
            {
                FK_basestationID = 1,
                Id = 11,
                Name = "EB01SE11",
                Location = null

            });
            var res = client.Post(json);

            Console.WriteLine(res.ResponseText);
        }
    }
}
