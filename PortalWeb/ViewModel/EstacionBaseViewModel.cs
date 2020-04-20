using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using Servicios;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace PortalWeb.ViewModel
{
    public class EstacionBaseViewModel : ComponentBase
    {
        public string nombreEstacionBase = "EB01";
        public ServicioEstacionBase servicio = new ServicioEstacionBase("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);

        public IEnumerable<EntidadSensorResultado> listaSensores = new List<EntidadSensorResultado>(); //lista de sensores de dicha estacion base
        
        public List<MapMarkerDataSource> MarkerDataSource = new List<MapMarkerDataSource>();
        public SfMaps mapa;

        protected override async Task OnInitializedAsync()
        {
            listaSensores = await servicio.ObtenerSensores(nombreEstacionBase);

            foreach (EntidadSensorResultado sensor in listaSensores)
            {
                MarkerDataSource.Add(new MapMarkerDataSource { latitude = Convert.ToDouble(sensor.Latitud), longitude = Convert.ToDouble(sensor.Longitud), name = sensor.NombreSensor });
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
    }
}