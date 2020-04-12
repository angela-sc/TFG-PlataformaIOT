using CoAP.Server.Resources;
using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.ApiCoap.Resources
{
	public class HelloWorldResource : Resource
	{
		private IServicioInsertaInformacion servicioInsertaInformacion;

		// use "helloworld" as the path of this resource
		public HelloWorldResource() : base("helloworld")
		{
			// set a friendly title
			Attributes.Title = "GET a friendly greeting!";
			//servicioInsertaInformacion = new ServicioInsertaInformacion("");
		}

		// override this method to handle GET requests
		protected override void DoGet(CoapExchange exchange)
		{
			// now we get a request, respond it
			exchange.Respond("Hello World!");
		}

		protected override void DoPost(CoAP.Server.Resources.CoapExchange exchange)
		{
			String payload = exchange.Request.PayloadString;
			EntidadPeticion entidadPeticion = JsonConvert.DeserializeObject<EntidadPeticion>(payload);
			Task.Run(async () => await servicioInsertaInformacion.InsertaPeticion(entidadPeticion));

			Console.WriteLine(entidadPeticion.Sensor);
			exchange.Respond(CoAP.StatusCode.Changed);
		}
	}
}
