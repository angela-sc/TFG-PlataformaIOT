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
        public IEnumerable<EntidadProyecto> proyectos;
        public ServicioProyecto servicio;

        private int usuario = InformacionUsuario.IdUsuario;

        // Initialize SearchTerm to "" to prevent null's
        public string SearchTerm { get; set; } = "";

        private string CadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";
        protected override async Task OnInitializedAsync()
        {
            proyectos = new List<EntidadProyecto>();
            servicio = new ServicioProyecto(CadenaConexion, null);

            proyectos = await servicio.ObtenerProyectos(usuario);
        }

        //Metodos para la búsqueda de proyectos+

        public List<EntidadProyecto> proyectosFiltrados => proyectos.Where(i => i.Nombre.ToLower().Contains(SearchTerm.ToLower())).ToList();
        

    }
}
