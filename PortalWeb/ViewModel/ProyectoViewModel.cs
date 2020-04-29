using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace PortalWeb.ViewModel
{
    public class ProyectoViewModel : ComponentBase
    {
        [Parameter]
        public string proyecto { get; set; } //nombre del proyecto

        //Lista con las estaciones base del proyecto
        protected IEnumerable<EntidadEstacionBase> estacionesBase;

        //private ServicioProyecto servicioProyecto;
        private IServicioEstacionBase servicioEstacionBase;

        protected override async Task OnInitializedAsync()
        {
            estacionesBase = new List<EntidadEstacionBase>();
            //servicio = new ServicioProyecto("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);
            servicioEstacionBase = new ServicioEstacionBase("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);

            //obtenemos las estaciones pertenecientes al proyecto
            estacionesBase = await servicioEstacionBase.ListaEstacionesBase(proyecto);
        }

        
    }
}
