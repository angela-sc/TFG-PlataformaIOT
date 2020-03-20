using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoAP;
using CoAP.Server;
using Libreria.Entidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EstacionBase.Ejemplo
{
    class Program
    {
        private static Uri uri = new Uri("coap://localhost:5683/COAPServer"); //URL donde montamos el servidor 
        private static string path = @"C:\Users\Ángela\Desktop\git-tfg\"; //Directorio donde estan los .txt

        public static async Task Main(String[] args) 
        {
            await PeticionPost();

        }


        private static async Task PeticionPost()
        {
            var cliente = new CoapClient();
            cliente.Uri = uri;

            var PeticionJSON = ObtenerDatos();

            Console.WriteLine(PeticionJSON);

            cliente.Post(PeticionJSON); //hacemos la peticion POST al servidor
        }

        private static String ObtenerDatos()
        {
            string peticion = null;
            //obtenemos los archivos que hay en el directorio
            var files = Directory.EnumerateFiles(path, "*.txt");

            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            foreach (string fileName in files)
            {
                List<EntidadDato> datos = new List<EntidadDato>();

                string eb = fileName.Substring(0,4); //nombre de la estacion base
                string se = fileName.Substring(4, 4); //nombre del sensor

                using(var sr = new StreamReader($@"{path}\{fileName}"))
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
                    EstacionBase = eb,
                    Sensor = se,
                    Datos = datos
                });

            }

            return peticion;
        }
    }

}
