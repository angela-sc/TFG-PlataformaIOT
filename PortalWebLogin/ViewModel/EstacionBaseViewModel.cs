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

        [Parameter]
        public string idEstacionBase { get; set; }        

        public IEnumerable<EntidadSensorResultado> listaSensores = new List<EntidadSensorResultado>(); //lista de sensores de dicha estacion base
        
        public List<MapMarkerDataSource> MarkerDataSource = new List<MapMarkerDataSource>();
        public SfMaps mapa;

        public double latitudInicial, longitudInicial;

        //> -- SERVICIO
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            Int32.TryParse(idEstacionBase, out int idEb);
            autorizado = await servicioEstacionBase.Autorizado(idUsuario, idEb);
        }

        protected override async Task OnInitializedAsync()
        {
            Int32.TryParse(idEstacionBase, out int id);
            nombreEstacionBase = await servicioEstacionBase.Nombre(id);
            listaSensores = await servicioEstacionBase.ObtenerSensores(id);

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