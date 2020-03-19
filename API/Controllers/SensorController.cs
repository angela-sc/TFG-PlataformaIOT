using CoAP.Server.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class SensorController : Resource
    {
        public SensorController(string name) : base(name)
        {
        }

        public SensorController(string name, bool visible) : base(name, visible)
        {
        }

        protected override void DoPost(CoapExchange exchange)
        {
            Console.WriteLine("Has llegado a la AP del SENSOR");
        }
    }
}
