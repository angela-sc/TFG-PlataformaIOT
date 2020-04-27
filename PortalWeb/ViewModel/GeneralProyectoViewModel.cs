using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class GeneralProyectoViewModel : ComponentBase
    {       
        public IEnumerable<EntidadProyecto> proyectos;
        public ServicioProyecto servicio;

        private int usuario = 1;

        // Initialize SearchTerm to "" to prevent null's
        public string SearchTerm { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            proyectos = new List<EntidadProyecto>();
            servicio = new ServicioProyecto("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);

            proyectos = await servicio.ObtenerProyectos(usuario);

        }

        //Metodos para la búsqueda de proyectos+

        public List<EntidadProyecto> proyectosFiltrados => proyectos.Where(i => i.name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        

    }
}
