using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var files = Directory.EnumerateFiles(path, "*.txt");
            
            foreach(string file in files)
            {
                var fileName = new FileInfo(file).Name;

                string peticion = ObtenerMetricas(fileName);
                var response = cliente.Post(peticion);

               if(response.StatusCode.ToString() == "Changed")
               {
                    File.Delete(file); //elimina el fichero
               }
               
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
                List<EntidadDatoBase> datos = new List<EntidadDatoBase>();

                //var fileName = new FileInfo(path).Name;
                var splittedFileName = fileName.Split('_', '.');
               
                using(var sr = new StreamReader($@"{path}{fileName}"))
                {
                    while (sr.Peek() > -1)
                    {
                        String linea = sr.ReadLine();
                        if (!String.IsNullOrEmpty(linea))
                        {
                            var datoJSON = JsonConvert.DeserializeObject<EntidadDatoBase>(linea, dateTimeConverter);
                            datos.Add(datoJSON);
                        }
                    }
                }

                peticion =  JsonConvert.SerializeObject(new EntidadPeticion()
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
