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
    public class ProyectoViewModel : ComponentBase
    {
        #region PARAMETROS
        [Parameter]
        public string proyecto { get; set; } //nombre del proyecto
        #endregion

        #region ATRIBUTOS PRIVADOS
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();
        #endregion

        protected IEnumerable<EntidadEstacionBase> estacionesBase; //Lista con las estaciones base del proyecto

        protected override async Task OnInitializedAsync()
        {           
            estacionesBase = new List<EntidadEstacionBase>();            
            //estacionesBase = await servicioEstacionBase.ListaEstacionesBase(proyecto); //obtenemos las estaciones pertenecientes al proyecto
        }        
    }
}
