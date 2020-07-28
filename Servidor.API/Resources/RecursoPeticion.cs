using CoAP.Server.Resources;
using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor.API.Resources
{
    public class RecursoPeticion : Resource
    {
        private IServicioInsertaInformacion servicioInsertaInformacion;
        private IServicioSeguridad servicioSeguridad;
        private Serilog.ILogger log;

        public RecursoPeticion() : base("COAPServer")
        {
            Attributes.Title = "Servidor COAP";

            this.log = FactoriaServicios.Log;
            servicioInsertaInformacion = FactoriaServicios.GetServicioInsertaInformacion();
            servicioSeguridad = FactoriaServicios.GetServicioSeguridad();

        }

        protected override void DoPost(CoapExchange exchange)
        {
            string payload = exchange.Request.PayloadString;

            if (payload != null)
            {
                try
                {
                    EntidadPeticionSegura peticionSegura = JsonConvert.DeserializeObject<EntidadPeticionSegura>(payload);  //Deserializamos la peticion en un objeto de tipo json
                    EntidadPeticion peticion = servicioSeguridad.ToEntidadPeticion(peticionSegura);

                    var insercion = Task.Run(async () => await servicioInsertaInformacion.InsertaPeticion(peticion));   //Lanzamos la peticion para que inserte los datos de forma asincrona
                    log.Information($"Resultado insercion (despues de Task.Run): {insercion.Result.ToString()}");
                    if (insercion.Result)
                    {
                        log.Information($"Datos insertados: {peticion.Proyecto}-{peticion.EstacionBase}-{peticion.Sensor} ({peticion.Datos.Count()})");
                        exchange.Respond(CoAP.StatusCode.Changed);
                    }
                    else
                    {
                        log.Error("ERR RECURSOPETICION (DoPost) - No se ha podido insertar la información en la base de datos.");
                        exchange.Respond(CoAP.StatusCode.BadRequest);
                    }
                }
                catch(Exception ex)
                {
                    log.Error($"ERR RECURSOPETICION (DoPost) - {ex.Message}");
                    exchange.Respond(CoAP.StatusCode.BadRequest);
                }                
            }
            else
            {
                log.Error("ERR RECURSOPETICION (DoPost) - La petición está vacía.");
                exchange.Respond(CoAP.StatusCode.BadRequest);
            }
        }

    }
}
