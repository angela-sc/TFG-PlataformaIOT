using CoAP.Server.Resources;
using Libreria.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.ApiCoap.Resources
{
	public class HelloWorldResource : Resource
	{
		// use "helloworld" as the path of this resource
		public HelloWorldResource() : base("helloworld")
		{
			// set a friendly title
			Attributes.Title = "GET a friendly greeting!";
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
			EntidadSensor entidadSensor = JsonConvert.DeserializeObject<EntidadSensor>(payload);
			exchange.Respond(payload);
		}
	}
}
