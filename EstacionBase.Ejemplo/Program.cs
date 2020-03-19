using System;
using CoAP;
using CoAP.Server;

namespace EstacionBase.Ejemplo
{
    class Program
    {
        static void Main(String[] args) 
        {
            Console.WriteLine("Hello Program!");


            //Peticion POST al "servidor" CoAP
            Request request = new Request(Method.POST);

            if(request == null)
            {
                Console.WriteLine("Petición vacía");
            }
            else
            {
                Console.WriteLine(request.Method);

                //URL del servidor
                request.SetUri("https://localhost:52832/");

                //datos a enviar en formato JSON
                request.SetPayload("{}");

                //Envio de la peticion al servidor
                request.Send();
                
            }

        }

    }

}
