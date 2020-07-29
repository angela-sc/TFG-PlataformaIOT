﻿using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PortalWebLogin.Data;
using Servicios;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace PortalWebLogin.ViewModel
{
    public class EstacionBaseViewModel : UsuarioAutenticadoViewModel
    {
        //[Parameter]
        public string nombreEstacionBase { get; set; }      

        public IEnumerable<EntidadSensorResultado> listaSensores = new List<EntidadSensorResultado>(); //lista de sensores de dicha estacion base
        
        public List<MapMarkerDataSource> MarkerDataSource = new List<MapMarkerDataSource>();
        public SfMaps mapa;

        public double latitudInicial, longitudInicial;

        //> -- SERVICIO
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();

        protected override async Task OnSecureParameterSetAsync()
        {
            if (autorizado)
            {
                Int32.TryParse(idEstacionBase, out int idEb);
                nombreEstacionBase = await servicioEstacionBase.Nombre(idEb);
                listaSensores = await servicioEstacionBase.ObtenerSensores(idEb);

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

                mapa.Theme = MapsTheme.Bootstrap4;
                mapa.Refresh(); // Refrescar mapa
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
                mapa.Refresh();
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