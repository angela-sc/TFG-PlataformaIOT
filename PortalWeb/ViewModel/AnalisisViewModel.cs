using Blazorise;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using PortalWeb.Data;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class AnalisisViewModel : ComponentBase
    {
        [Parameter]
        public string proyecto { get; set; } //nombre del proyecto, unico en la tabla 'Proyecto'

        protected List<EntidadDatoBase> aux;



        private IEnumerable<EntidadEstacionBase> estacionesbase = null;
        private IEnumerable<EntidadSensorResultado> sensores = null;
        
        private IServicioProyecto servicioP;
        private IServicioEstacionBase servicioEB;
        private IServicioSensor servicioS;


        private int idUsuario = InformacionUsuario.IdUsuario;
        private string CadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";

        protected override async Task OnInitializedAsync()
        {
            aux = new List<EntidadDatoBase>();

            servicioP = new ServicioProyecto(CadenaConexion, null);
            servicioEB = new ServicioEstacionBase(CadenaConexion, null);
            servicioS = new ServicioSensor(CadenaConexion, null);

            estacionesbase = new List<EntidadEstacionBase>();
            //estacionesbase = await servicioEB.ListaEstacionesBase(proyecto);

            sensores = new List<EntidadSensorResultado>();
            foreach(var estacion in estacionesbase)
            {
                var s = await servicioEB.ObtenerSensores(estacion.Id);
                foreach(var sensor in s)
                {
                    var datos = await servicioS.ObtenerDatos(sensor.IdSensor, 10);

                   foreach(var d in datos)
                    {
                        aux.Add(d);
                    }
                   
                }                                           
            }

            this.StateHasChanged();          
        }
    }
}
