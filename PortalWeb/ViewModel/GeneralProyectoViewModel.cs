using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PortalWeb.Data;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class GeneralProyectoViewModel : ComponentBase
    {       
        protected IEnumerable<EntidadProyecto> proyectos;
        protected IServicioProyecto servicioProyecto = FactoriaServicios.GetServicioProyecto();
        public string SearchTerm { get; set; } = "";  // Initialize SearchTerm to "" to prevent null's

        // > -- ATRIBUTOS PRIVADOS
        private int usuario = InformacionUsuario.IdUsuario;

        protected override async Task OnInitializedAsync()
        {
            proyectos = new List<EntidadProyecto>();          
            proyectos = await servicioProyecto.ObtenerProyectos(usuario);

            this.StateHasChanged();
        }

        //Metodos para la búsqueda de proyectos+
        public List<EntidadProyecto> proyectosFiltrados => proyectos.Where(i => i.Nombre.ToLower().Contains(SearchTerm.ToLower())).ToList();
        

    }
}
