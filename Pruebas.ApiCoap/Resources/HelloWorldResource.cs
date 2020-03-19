using CoAP.Server.Resources;
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
	}
}
