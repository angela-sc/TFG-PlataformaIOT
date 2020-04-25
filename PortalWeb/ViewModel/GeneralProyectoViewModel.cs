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

        protected override async Task OnInitializedAsync()
        {
            proyectos = new List<EntidadProyecto>();
            servicio = new ServicioProyecto("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);

            proyectos = await servicio.ObtenerProyectos(usuario);

        }
    }
}
