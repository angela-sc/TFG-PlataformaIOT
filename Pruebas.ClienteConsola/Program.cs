using CoAP;
using Libreria.Entidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.ClienteConsola
{
    class Program
    {
        private static Uri uri = new Uri("coap://localhost:5683/helloworld"); //URL donde montamos el servidor 
        private static string path = @"C:\Users\abox\Downloads\"; //Directorio donde estan los .txt

        public static async Task Main(string[] args)
        {
            //await TestGet_ApiNormal();
            //await TestGet_ApiCoap();
            //await TestPost_ApiCoap();
            TestPostMessage_ApiCoap();
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

        private static void TestPostMessage_ApiCoap()
        {
            var cliente = new CoapClient();
            cliente.Uri = uri;

            var files = Directory.EnumerateFiles(path, "*.txt");
            foreach (string file in files)
            {
                var fileName = new FileInfo(file).Name;

                var peticion = ObtenerMetricas(fileName);
                var response = cliente.Post(peticion);

                Console.WriteLine(fileName + " response: " + response.StatusCode.ToString());
            }
        }

        private static String ObtenerMetricas(string fileName)
        {
            String peticion = null;

            //var files = Directory.EnumerateFiles(path, "*.txt");

            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            //foreach (string path in files)
            //{
            List<EntidadDato> datos = new List<EntidadDato>();

            //var fileName = new FileInfo(path).Name;
            var splittedFileName = fileName.Split('_', '.');

            using (var sr = new StreamReader($@"{path}\{fileName}"))
            {
                while (sr.Peek() > -1)
                {
                    String linea = sr.ReadLine();
                    if (!String.IsNullOrEmpty(linea))
                    {
                        var datoJSON = JsonConvert.DeserializeObject<EntidadDato>(linea, dateTimeConverter);
                        datos.Add(datoJSON);
                    }
                }
            }

            peticion = JsonConvert.SerializeObject(new EntidadPeticion()
            {
                EstacionBase = splittedFileName.First(),
                Sensor = splittedFileName.ElementAt(1),
                Datos = datos
            });

            //}

            return peticion;
        }
    }
}
