using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoAP.Server.Resources;
using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Serilog;
using Servicios;

namespace API.Resources
{
    public class RecursoPeticion : Resource
    {
        private IServicioInsertaInformacion servicioInsertaInformacion;
        private ILogger log;

        public RecursoPeticion(ILogger logger) : base("COAPServer")
        {
            this.log = logger;
            
            Attributes.Title = "Servidor CoAP";
  
            servicioInsertaInformacion = new ServicioInsertaInformacion(log);
        }

        protected override void DoPost(CoapExchange exchange)
        {
            string payload = exchange.Request.PayloadString;

            if(payload != null)
            {
                //Deserializamos la peticion en un objeto de tipo json
                EntidadPeticion entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(payload);

                //Lanzamos la peticion para que inserte los datos de forma asincrona
                var a =  Task.Run(async () => await servicioInsertaInformacion.InsertaPeticion(entidadPeticion));
                
                if (a.Result)
                {
                    exchange.Respond(CoAP.StatusCode.Changed);
                }
                else
                {
                    log.Error("Error al insertar la información en la base de datos.");
                    exchange.Respond(CoAP.StatusCode.NotAcceptable);
                }
            }
            else
            {
                //TODO: IMPLEMENTAR EN CASO DE ERROR > registrar en el log de la api
                log.Error("Petición vacía");             
            }
        }
    }
}
