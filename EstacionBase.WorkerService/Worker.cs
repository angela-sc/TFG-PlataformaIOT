using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Servicios;

using Serilog;

namespace EstacionBase.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly Uri uri; //URL donde montamos el servidor 
        private readonly string directorioSensores; //Directorio donde estan los .txt
        private readonly int tiempoEnvio;

        private CoapClient client;

        private IServicioSeguridad servicioSeguridad;
        
        public Worker(ILogger<Worker> logger)
        {
            if (FactoriaServicios.Log == null)
                throw new ArgumentNullException("Log - {appsettings.json}");
            if(FactoriaServicios.UriCOAP == null)
                throw new ArgumentNullException("URI COAP vacía.");
            if(string.IsNullOrEmpty(FactoriaServicios.DirectorioSensores))
                throw new ArgumentNullException("DirectorioSensores - {appsettings.json}");            

            _logger = FactoriaServicios.Log;
            uri = FactoriaServicios.UriCOAP;
            directorioSensores = FactoriaServicios.DirectorioSensores;

            tiempoEnvio = FactoriaServicios.GetTiempoEnvio();
        }

        //Inicializo el cliente cuando arranca el servicio
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new CoapClient();
            client.Uri = uri;

            _logger.Information("COAP uri: " + uri.ToString());

            servicioSeguridad = FactoriaServicios.GetServicioSeguridad();

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var files = Directory.EnumerateFiles(directorioSensores, "*.txt");

                    foreach (string file in files)
                    {
                        var fileName = new FileInfo(file).Name;

                        string payload = GetData(fileName);
                        if (!string.IsNullOrEmpty(payload))
                        {
                            var result = client.Post(payload);

                            if (result.StatusCode.ToString() == "Changed")
                            {                              
                                _logger.Information($"WORKER (ExecuteAsync) - La informacion del fichero {fileName} se ha insertado correctamente");
                                
                                File.Delete(file); //elimina el fichero
                            }
                            else
                            {                                
                                _logger.Error($"ERR. WORKER (ExecuteAsync) - No se ha podido insertar la información del fichero {fileName}");
                                File.Delete(file); //elimina el fichero
                            }
                        }
                    }
                }
                catch(Exception ex)
                {                    
                    _logger.Error($"ERR WORKER (ExecuteAsync) - {ex.Message}");
                }
                
                await Task.Delay(tiempoEnvio * 1000, stoppingToken);              
            }
        }

        private string GetData(string fileName)
        {
            string request;
            
            var dateFormat = "yyyy-MM-dd HH:mm:ss"; //formato utilizado al componer el json en el campo stamp
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            List<EntidadDatoBase> data = new List<EntidadDatoBase>();
            
            var splittedFileName = fileName.Split('_', '.');

            try
            {
                using (var sr = new StreamReader($@"{directorioSensores}{fileName}"))
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

                    EntidadPeticion peticion = new EntidadPeticion()
                    {
                        Proyecto = FactoriaServicios.Proyecto,
                        EstacionBase = FactoriaServicios.EstacionBase,
                        Sensor = splittedFileName.ElementAt(0),
                        Datos = data
                    };

                    EntidadPeticionSegura peticionSegura = servicioSeguridad.ToEntidadPeticionSegura(peticion);
                    request = JsonConvert.SerializeObject(peticionSegura);
                }
            }catch(Exception ex)
            {
                request = null;
                _logger.Error($"ERR WORKER (GetData) - {ex.Message}");
            }
            return request;
        }
    }
}

