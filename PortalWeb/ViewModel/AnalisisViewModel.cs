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
        protected IEnumerable<EntidadProyecto> proyectos;
        

        private IServicioProyecto servicioProyecto;
        private int idUsuario;

        protected override async Task OnInitializedAsync()
        {
            idUsuario = InformacionUsuario.IdUsuario;
            servicioProyecto = new ServicioProyecto("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);

            proyectos = new List<EntidadProyecto>();
            proyectos = await servicioProyecto.ObtenerProyectos(idUsuario);

            this.StateHasChanged();
        }
    }
}
