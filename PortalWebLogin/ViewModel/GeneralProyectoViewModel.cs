using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWebLogin.Data;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebLogin.ViewModel
{
    public class GeneralProyectoViewModel : UsuarioAutenticadoViewModel
    {       
        protected IEnumerable<EntidadProyecto> proyectos;
        public List<Tuple<int, EntidadEstacionBase>> listaEstacionesBase;
        protected string SearchTerm { get; set; } = "";  // Initialize SearchTerm to "" to prevent null's

        // > -- ATRIBUTOS PRIVADOS
        private IServicioProyecto servicioProyecto;// = FactoriaServicios.GetServicioProyecto();
        private IServicioEstacionBase servicioEstacionBase; // = FactoriaServicios.GetServicioEstacionBase();
        private IServicioSensor servicioSensor; // = FactoriaServicios.GetServicioSensor(); // -- añadido hoy       

        protected override async Task OnInitializedAsync()
        {
            servicioProyecto = FactoriaServicios.GetServicioProyecto();
            servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();
            servicioSensor = FactoriaServicios.GetServicioSensor();

            proyectos = new List<EntidadProyecto>();          
            proyectos = await servicioProyecto.ObtenerProyectos(idUsuario);

            this.StateHasChanged();
        }

        //Metodos para la búsqueda de proyectos+
        public List<EntidadProyecto> proyectosFiltrados => proyectos.Where(i => i.Nombre.ToLower().Contains(SearchTerm.ToLower())).ToList();


        // Metodos para mostrar las tarjetas de estaciones base
        protected bool mostrar = false;
        protected string nombreProyecto; 
        public async Task Mostrar(EntidadProyecto proyecto)
        {
            nombreProyecto = proyecto.Nombre;
            var estacionesBase = await servicioEstacionBase.ListaEstacionesBase(proyecto.Id);

            listaEstacionesBase = new List<Tuple<int, EntidadEstacionBase>>();
            foreach(var estacion in estacionesBase)
            {
                var sensores = await servicioEstacionBase.ObtenerSensores(estacion.Id);
                listaEstacionesBase.Add(new Tuple<int, EntidadEstacionBase>(sensores.Count(), estacion));
            }
            
            this.mostrar = true;
            this.StateHasChanged();
        }

    }
}
