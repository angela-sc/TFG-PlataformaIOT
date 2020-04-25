using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Servicios;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace PortalWeb.ViewModel
{
    public class EstacionBaseViewModel : ComponentBase
    {
        [Parameter]
        public string nombreEstacionBase { get; set; }

        //public string nombreEstacionBase = "EB01";
        public ServicioEstacionBase servicio = new ServicioEstacionBase("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);

        public IEnumerable<EntidadSensorResultado> listaSensores = new List<EntidadSensorResultado>(); //lista de sensores de dicha estacion base
        
        public List<MapMarkerDataSource> MarkerDataSource = new List<MapMarkerDataSource>();
        public SfMaps mapa;

        public double latitudInicial, longitudInicial;

        protected override async Task OnInitializedAsync()
        {
            listaSensores = await servicio.ObtenerSensores(nombreEstacionBase);

            //que pasa si la estacion base no tiene sensores -> devuelve una lista null
            
                foreach (EntidadSensorResultado sensor in listaSensores)
                {

                    if (sensor.Fecha == default(DateTime))
                    {
                        MarkerDataSource.Add(new MapMarkerDataSource { latitude = Convert.ToDouble(sensor.Latitud), longitude = Convert.ToDouble(sensor.Longitud), name = sensor.NombreSensor, color = "red" });
                    }
                    else
                    {
                        MarkerDataSource.Add(new MapMarkerDataSource { latitude = Convert.ToDouble(sensor.Latitud), longitude = Convert.ToDouble(sensor.Longitud), name = sensor.NombreSensor, color = "green" });
                    }
                }
            if (listaSensores.Count() > 0)
            {
                latitudInicial = MarkerDataSource.ElementAt(0).latitude;
                longitudInicial = MarkerDataSource.ElementAt(0).longitude;
                
            }
            else
            {
                //latitud: [0 a 90] [0 a -90]
                latitudInicial = 99.0;

                //longitud: [0 a 180] [0 a -180]
                longitudInicial = 199;

            }
            mapa.Refresh(); // Refrescar mapa
        }

        
    }

    //Clase "intermedia" utilizada para añadir marcadores al mapa
    public class MapMarkerDataSource
    {
        public string name;
        public double latitude;
        public double longitude;
        public string color; 
    }
}