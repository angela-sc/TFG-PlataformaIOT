using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using PortalWeb.Model;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class AdministrarProyectosViewModel : ComponentBase
    {
        public Proyecto Proyecto;
    
        protected IEnumerable<EntidadProyecto> proyectos = null; //lista de todos los proyectos del usuario

        public bool creado = false; //indica si se ha creado o no el proyecto
        public bool crear = false;  //indica si se va a crear un proyecto o no

        public bool editar, editado = false; //indican si se va a editar un proyecto y si se ha editado

        public bool eliminar, eliminado = false; //indica si se ha pulsado el boton eliminar y si se ha eliminado un proyecto

        private string CadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";

        protected ServicioProyecto servicio;

        protected override async Task OnInitializedAsync()
        {
            servicio = new ServicioProyecto(CadenaConexion, null);

            int id = 1; //ide del usuario -> BORRAR
            proyectos = await servicio.ObtenerProyectos(id);

            this.StateHasChanged();
        }

        protected async Task Crear()
        {
            Console.WriteLine("Función crear activada.");
            Proyecto = new Proyecto();
            this.crear = true;
        }
        public async Task CrearProyecto()
        {
            Console.WriteLine("Función crear proyecto activada.");
            servicio = new ServicioProyecto(CadenaConexion, null);

            servicio.CrearProyecto(new EntidadProyecto()
            {
                Nombre = Proyecto.Nombre,
                Desrcipcion = Proyecto.Descripcion
            });

            creado = true;
        }

        protected async Task Editar()
        {
            Console.WriteLine("Función editar activada.");
            //Proyecto = new Proyecto();
            this.editar = true;
            
        }
        protected async Task Eliminar()
        {
            Console.WriteLine("Función eliminar activada.");
            //Proyecto = new Proyecto();
            this.eliminar = true;
        }

        protected async Task EliminarProyecto()
        {
            Console.WriteLine("Función 'eliminar proyecto' activada.");
            //Proyecto = new Proyecto();

           //await servicio.EliminarSensor(2,5014);
            this.eliminado = true;
            
           

            this.StateHasChanged(); //el componente debe refrescarse para mostrar la vista sin el proyecto
        }

        protected async Task Close()
        {
            this.crear = false;
            this.creado = false;

            this.editar = false;
            this.editado = false;

            this.eliminar = false;
            this.eliminado = false;

            this.StateHasChanged();
        }

        
    }
      
}
