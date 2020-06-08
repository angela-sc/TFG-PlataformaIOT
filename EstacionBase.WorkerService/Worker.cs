using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using Libreria.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EstacionBase.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        private CoapClient client;
        private readonly Uri uri; //URL donde montamos el servidor 
        private readonly string path; //Directorio donde estan los .txt

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var config = Program.GetConfiguration();
            uri = new Uri(config["CoapUri"]);
            path = config["SensorFilesDirectory"];
        }

        //Inicializo el cliente cuando arranca el servicio
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new CoapClient();
            client.Uri = uri;

            _logger.LogInformation("COAP uri: " + uri.ToString());

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var files = Directory.EnumerateFiles(path, "*.txt");
                    //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    //acción que queremos ejecutar, post > PostPetition();
                    foreach (string file in files)
                    {
                        var fileName = new FileInfo(file).Name;

                        string payload = GetData(fileName);
                        if (!string.IsNullOrEmpty(payload))
                        {
                            var result = client.Post(payload);

                            //Console.WriteLine(result.StatusCode);
                            if (result.StatusCode.ToString() == "Changed")
                            {
                                //_logger.LogInformation($"Sensor data ({fileName}) has been inserted correctly. Status code {result.StatusCode}", fileName, result.StatusCode);
                                _logger.LogInformation($"WORKER (ExecuteAsync) - La informacion del fichero {fileName} se ha insertado correctamente");
                                File.Delete(file); //elimina el fichero
                            }
                            else
                            {
                                //_logger.LogError($"An error occurred while inserting data from the {fileName} file. Status code {result.StatusCode}", fileName, result.StatusCode);
                                _logger.LogError($"ERR. WORKER (ExecuteAsync) - No se ha podido insertar la información del fichero {fileName}");
                                File.Delete(file); //elimina el fichero
                            }
                        }
                        //con _logger.LogInformation("...",result.StatusCode); 
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError("Error al leer el fichero: " + ex.Message);
                }
                
                await Task.Delay(60 * 1000, stoppingToken);
                //await Task.Delay(300*1000, stoppingToken);                
            }
        }

        /*private async Task PostPetition()
        {
            var files = Directory.EnumerateFiles(path, "*.txt");

            foreach(string file in files)
            {
                var fileName = new FileInfo(file).Name;
                string request = GetData(fileName);

                var response = client.Post(request);

            }
        }*/

        private string GetData(string fileName)
        {
            string request = null;

            //formato utilizado al componer el json en el campo stamp
            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            //var dateFormat = "dd-MM-yyy HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            List<EntidadDatoBase> data = new List<EntidadDatoBase>();
            
            var splittedFileName = fileName.Split('_', '.');

            try
            {
                using (var sr = new StreamReader($@"{path}{fileName}"))
                {
                    while (sr.Peek() > -1)
                    {
                        string linea = sr.ReadLine();
                        if (!string.IsNullOrEmpty(linea))
                        {
                            var datoJSON = JsonConvert.DeserializeObject<EntidadDatoBase>(linea, dateTimeConverter);
                            data.Add(datoJSON);
                        }
                    }

                    request = JsonConvert.SerializeObject(new EntidadPeticion()
                    {
                        Proyecto = Program.GetConfiguration().GetValue<string>("Proyecto"), // LEER DEL APPSETTINGS
                        //EstacionBase = splittedFileName.First(), // CAMBIAR PARA QUE COJA EL NOMBRE DEL APPSETTINGS
                        EstacionBase = Program.GetConfiguration().GetValue<string>("EstacionBase"),
                        Sensor = splittedFileName.ElementAt(0),
                        //Sensor = splittedFileName.ElementAt(1), // SERÁ EL NOMBRE DEL ARCHIVO
                        Datos = data
                    });
                }
            }catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //_logger.LogError("An error ocurred when getting data: ", ex.Message);
                _logger.LogError(ex.Message, "An error ocurred when getting data: ");
            }
            return request;
        }
    }
}

