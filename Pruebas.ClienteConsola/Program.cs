using System;
using System.Threading.Tasks;

namespace Pruebas.ClienteConsola
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //await TestGet_ApiNormal();
            await TestGet_ApiCoap();
        }

        private static async Task TestGet_ApiNormal()
        {
            var respuesta = await ClientHelper.GetAsync();
            Console.WriteLine(respuesta);
        }

        private static async Task TestGet_ApiCoap()
        {

        }
    }
}
