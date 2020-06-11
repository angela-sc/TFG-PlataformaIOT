using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using Libreria.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Servicios;

namespace EstacionBase.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Uri uri; //URL donde montamos el servidor 
        private readonly string path; //Directorio donde estan los .txt

        private CoapClient client;

        private ServicioSeguridad seguridad;
        
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
           
            var config = Program.GetConfiguration();
            uri = new Uri(config["UriCoap"]); //uri = new Uri(Program.GetConfiguration().GetValue<string>("CoapUri"));
            path = config["DirectorioFicherosSensores"];

            
        }

        //Inicializo el cliente cuando arranca el servicio
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new CoapClient();
            client.Uri = uri;

            _logger.LogInformation("COAP uri: " + uri.ToString());

            seguridad = new ServicioSeguridad("C:\\tfg\\claves\\clave_publica.key", null);

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

                    //accion que queremos ejecutar, post > PostPetition();
                    foreach (string file in files)
                    {
                        var fileName = new FileInfo(file).Name;

                        string payload = GetData(fileName);
                        //var size = Encoding.ASCII.GetBytes(payload);
                        if (!string.IsNullOrEmpty(payload))
                        {
                            //var cifrado = seguridad.CifrarRSA(payload);
                            
                            var result = client.Post(payload);

                            if (result.StatusCode.ToString() == "Changed")
                            {                              
                                _logger.LogInformation($"WORKER (ExecuteAsync) - La informacion del fichero {fileName} se ha insertado correctamente");
                                
                                File.Delete(file); //elimina el fichero
                            }
                            else
                            {                                
                                _logger.LogError($"ERR. WORKER (ExecuteAsync) - No se ha podido insertar la información del fichero {fileName}");
                                File.Delete(file); //elimina el fichero
                            }
                        }
                        //con _logger.LogInformation("...",result.StatusCode); 
                    }
                }
                catch(Exception ex)
                {                    
                    _logger.LogError($"ERR WORKER (ExecuteAsync) - {ex.Message}");
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
            string request;
            
            var dateFormat = "yyyy-MM-dd HH:mm:ss"; //formato utilizado al componer el json en el campo stamp
            //var dateFormat = "dd-MM-yyyy HH:mm:ss";
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
                        Proyecto = Program.GetConfiguration().GetValue<string>("Proyecto"),
                        EstacionBase = Program.GetConfiguration().GetValue<string>("EstacionBase"),
                        Sensor = splittedFileName.ElementAt(0),
                        Datos = data
                    });
                }
            }catch(Exception ex)
            {
                request = null;
                _logger.LogError($"ERR WORKER (GetData) - {ex.Message}");
            }
            return request;
        }
    }
}

