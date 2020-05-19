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
        
        protected IEnumerable<EntidadEstacionBase> estacionesBase; //Lista con las estaciones base del proyecto

        private IServicioEstacionBase servicioEstacionBase;
        private string CadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";

        protected override async Task OnInitializedAsync()
        {
            estacionesBase = new List<EntidadEstacionBase>();

            servicioEstacionBase = new ServicioEstacionBase(CadenaConexion, null);    
            
            estacionesBase = await servicioEstacionBase.ListaEstacionesBase(proyecto); //obtenemos las estaciones pertenecientes al proyecto
        }        
    }
}
