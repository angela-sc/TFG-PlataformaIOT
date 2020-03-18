using System;
using System.Collections.Generic;
using CoAP;

namespace EstacionBase.Ejemplo
{
    class ClienteEjemplo
    {
        public static void Main(String[] args)
        {
            //Peticion POST
            Request request = new Request(Method.POST);
            request.SetUri("coap://[::1]/hello-world"); //URL del servidor
            request.SetPayload("{}"); //se enviaría en JSON

            request.Send();

            //Respuesta
            // receive response and check
            Response response = request.WaitForResponse(100);
            /*Assert.IsNotNull(response);
            Assert.AreEqual(response.PayloadString, SERVER_RESPONSE);
            Assert.AreEqual(response.Type, MessageType.CON);*/
        }
    }
}
