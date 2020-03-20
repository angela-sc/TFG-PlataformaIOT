using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoAP.Server.Resources;
using Libreria.Entidades;
using Newtonsoft.Json;

namespace API.Resources
{
    public class RecursoPeticion : Resource
    {
        public RecursoPeticion() : base("COAPServer")
        {
            Attributes.Title = "PETICION COAP";
        }

        protected override void DoPost(CoapExchange exchange)
        {

            String payload = exchange.Request.PayloadString;
            EntidadPeticion entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(payload);

           
            Console.WriteLine("Objeto deserializado: \n");



            Console.WriteLine($"Estacion base: " + entidadPeticion.EstacionBase);
            Console.WriteLine("Sensor: " + entidadPeticion.Sensor);
            foreach (EntidadDato ed in entidadPeticion.Datos)
            {
                Console.WriteLine(ed.stamp);
            }
        }
    }
}
