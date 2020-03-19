using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ClienteConsola
{
    public class ClientHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string ApiUri = "http://localhost:65243";

        public static async Task<string> GetAsync()
        {
            var responseString = await client.GetStringAsync($"{ApiUri}/api/WeatherForecast");
            return responseString;
        }

        public static async Task<string> PostAsync()
        {
            var values = new Dictionary<string, string>
            {
            { "thing1", "hello" },
            { "thing2", "world" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"{ApiUri}/recepticle.aspx", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
