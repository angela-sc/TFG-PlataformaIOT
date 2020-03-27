using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using Libreria.Entidades;
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

        private static Uri uri = new Uri("coap://localhost:5683/COAPServer"); //URL donde montamos el servidor 
        private static string path = @"C:\Users\Ángela\Desktop\git-tfg\"; //Directorio donde estan los .txt

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        //Inicializo el cliente cuando arranca el servicio
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new CoapClient();
            client.Uri = uri;

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var files = Directory.EnumerateFiles(path, "*.txt");

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //acción que queremos ejecutar, post > PostPetition();
                foreach (string file in files)
                {
                    var fileName = new FileInfo(file).Name;

                    string payload = GetData(fileName);
                    var result = client.Post(payload);

                    //Console.WriteLine(result.StatusCode);
                    if(result.StatusCode.ToString() == "Changed")
                    {
                        _logger.LogInformation("Sensor data ({fileName}) has been inserted correctly. Status code {StatusCode}", fileName, result.StatusCode);
                    }
                    else
                    {
                        _logger.LogError("An error occurred while inserting data from the {fileName} file. Status code {StatusCode}", fileName, result.StatusCode);
                    }

                    //con _logger.LogInformation("...",result.StatusCode); 
                }

                //await Task.Delay(300*1000, stoppingToken);
                await Task.Delay(60 * 1000, stoppingToken);
            }

        }

        private async Task PostPetition()
        {
            var files = Directory.EnumerateFiles(path, "*.txt");

            foreach(string file in files)
            {
                var fileName = new FileInfo(file).Name;
                string request = GetData(fileName);

                var response = client.Post(request);

            }
        }

        private static string GetData(string fileName)
        {
            string request = null;

            //formato utilizado al componer el json en el campo stamp
            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            List<EntidadDatoBase> data = new List<EntidadDatoBase>();
            
            var splittedFileName = fileName.Split('_', '.');

            try
            {
                using (var sr = new StreamReader($@"{path}\{fileName}"))
                {
                    while (sr.Peek() > -1)
                    {
                        String linea = sr.ReadLine();
                        if (!String.IsNullOrEmpty(linea))
                        {
                            var datoJSON = JsonConvert.DeserializeObject<EntidadDatoBase>(linea, dateTimeConverter);
                            data.Add(datoJSON);
                        }
                    }

                    request = JsonConvert.SerializeObject(new EntidadPeticion()
                    {
                        EstacionBase = splittedFileName.First(),
                        Sensor = splittedFileName.ElementAt(1),
                        Datos = data
                    });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return request;
        }
    }
}

