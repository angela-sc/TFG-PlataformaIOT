using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoAP.Server.Resources;
using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Servicios;

namespace API.Resources
{
    public class RecursoPeticion : Resource
    {
        private IServicioInsertaInformacion servicioInsertaInformacion;
        
        public RecursoPeticion() : base("COAPServer")
        {
            Attributes.Title = "Servidor CoAP";
            servicioInsertaInformacion = new ServicioInsertaInformacion();
        }

        //protected override void DoPost(CoAP.Server.Resources.CoapExchange exchange)
        //{
        //    String payload = exchange.Request.PayloadString;
        //    if(payload == null)
        //    {
        //        Console.WriteLine("PETICION VACIA");
        //    }
        //    EntidadPeticion entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(payload);


        //    Console.WriteLine("Objeto deserializado: \n");

        //    Console.WriteLine($"Estacion base: " + entidadPeticion.EstacionBase);
        //    Console.WriteLine("Sensor: " + entidadPeticion.Sensor);
        //    foreach (EntidadDato ed in entidadPeticion.Datos)
        //    {
        //        Console.WriteLine(ed.stamp);
        //    }
        //}

        protected override void DoPost(CoapExchange exchange)
        {
            string payload = exchange.Request.PayloadString;

            if(payload != null)
            {
                EntidadPeticion entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(payload);
                Task.Run(async () => await servicioInsertaInformacion.InsertaPeticion(entidadPeticion));

                exchange.Respond(CoAP.StatusCode.Changed);

            }
            else
            {
                //TODO: IMPLEMENTAR EN CASO DE ERROR
            }
        }

    }
}
