﻿using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using PortalWeb.Model;
using PortalWeb.Pages;
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
        private ModeloEstacionBase estacionbase = null;        
        protected ModeloEstacionBase EstacionBaseEditar  //Atributo que nos sirve para editar
        { 
            get 
            { 
                return estacionbase; 
            }
            set 
            {
                estacionbase = value;
                this.StateHasChanged();
            }
        }

        protected ModeloProyecto Proyecto;

        protected enum EntidadTratada
        {
            PROYECTO,
            ESTACIONBASE,
            SENSOR
        }

        protected string claseModal = "";
        private static readonly string modalCorrecto = "alert alert-success", modalError = "alert alert-danger";

        protected string mensajeEliminar; //mensaje que se publica en el pop up segun si se ha eliminado el elemento o no
        protected string preguntaEliminar;
        protected string encabezadoEliminar; 


        protected IEnumerable<EntidadProyecto> proyectos = null; //lista de todos los proyectos del usuario
        protected List<EntidadEstacionBase> estaciones;
        protected List<EntidadSensorResultado> sensores;

        public bool creado = false; //indica si se ha creado o no el proyecto
        public bool crear = false;  //indica si se va a crear un proyecto o no

        public bool editar, editado = false; //indican si se va a editar un proyecto y si se ha editado

        public bool eliminar, eliminado = false; //indica si se ha pulsado el boton eliminar y si se ha eliminado un proyecto
        
        public int idEliminar; //id del elemento (proyecto, estacion o sensor) que se va a eliminar
        protected EntidadTratada entidadEliminar;

        protected bool editarProyecto, editarEstacionBase, editarSensor = false;

        private string CadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true";

        private ServicioProyecto servicioProyecto;
        private ServicioEstacionBase servicioEstacionBase;
        private ServicioSensor servicioSensor;

        private int idUsuario; // usuario logado

        protected override async Task OnInitializedAsync()
        {
            servicioProyecto = new ServicioProyecto(CadenaConexion, null);
            servicioEstacionBase = new ServicioEstacionBase(CadenaConexion, null);
            servicioSensor = new ServicioSensor(CadenaConexion, null);

            idUsuario = 1; //ide del usuario -> BORRAR

            await CargarDatos();
            
            this.StateHasChanged();
        }

        protected async Task Crear()
        {
            Console.WriteLine("Función crear activada.");
            Proyecto = new ModeloProyecto();
            this.crear = true;
        }
        public async Task CrearProyecto()
        {
            Console.WriteLine("Función crear proyecto activada.");
            servicioProyecto = new ServicioProyecto(CadenaConexion, null);

           await servicioProyecto.CrearProyecto(new EntidadProyecto()
            {
                Nombre = Proyecto.Nombre,
                Descripcion = Proyecto.Descripcion
            });

            creado = true;
        }

        protected async Task Editar(ModeloProyecto proyecto)
        {
            Console.WriteLine("Función editar activada.");

            Proyecto = new ModeloProyecto()
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                Descripcion = string.IsNullOrEmpty(proyecto.Descripcion) ? "" : proyecto.Descripcion
                
            };

            this.editar = true;            
        }

        
        protected void ActivarEditar(EntidadEstacionBase eb) //Activa el modal, que se muestra cuando EstacionBaseEditar != null
        {
            EstacionBaseEditar = new ModeloEstacionBase()
            {
                Id = eb.Id,
                Nombre = eb.Nombre
            };
        }
        protected async Task EditarEstacionBase() 
        {
            var estacion = new EntidadEstacionBase()
            {
                Id = EstacionBaseEditar.Id,
                Nombre = EstacionBaseEditar.Nombre                
            };

            await servicioEstacionBase.Editar(estacion);

            this.editado = true;
            this.StateHasChanged();
        }

        protected async Task ActivarEliminar(EntidadTratada entidad, int id)
        {
            Console.WriteLine("Función eliminar activada.");

            entidadEliminar = entidad;
            if(entidadEliminar == EntidadTratada.PROYECTO)
            {
                encabezadoEliminar = "Eliminar proyecto";
                preguntaEliminar = "¿Seguro que desea eliminar el proyecto?";

            }
            else if (entidadEliminar == EntidadTratada.ESTACIONBASE) 
            {
                encabezadoEliminar = "Eliminar estacion base";
                preguntaEliminar = "¿Seguro que desea eliminar la estación base?";
            }
            else
            {
                encabezadoEliminar = "Eliminar sensor";
                preguntaEliminar = "¿Seguro que desea eliminar el sensor?";
            }
            
            idEliminar = id;

            this.eliminar = true;
            this.eliminado = false;
        }

      

        protected async Task Eliminar()
        {
            this.eliminado = true;

            bool resultadoBorrado;
            if (entidadEliminar == EntidadTratada.PROYECTO)
            { 
                resultadoBorrado = await servicioProyecto.EliminarProyecto(idEliminar);
                if (resultadoBorrado)
                {
                    mensajeEliminar = "El proyecto se ha eliminado correctamente.";
                    claseModal = modalCorrecto;
                }
                else
                {
                    mensajeEliminar = "No se ha podido eliminar el proyecto.";
                    claseModal = modalError;
                }
            }
            else if(entidadEliminar == EntidadTratada.ESTACIONBASE)
            {
                resultadoBorrado = await servicioEstacionBase.EliminarEstacionBase(idEliminar);

                if (resultadoBorrado)
                {
                    mensajeEliminar = "La estación base se ha  eliminado correctamente.";
                    claseModal = modalCorrecto;
                }
                else
                {
                    mensajeEliminar = "No se ha podido eliminar la estación base.";
                    claseModal = modalError;
                }
            }
            else
            {
                resultadoBorrado = await servicioSensor.EliminarSensor(idEliminar);

                if (resultadoBorrado)
                {
                    mensajeEliminar = "El sensor se ha  eliminado correctamente.";
                    claseModal = modalCorrecto;
                }
                else
                {
                    mensajeEliminar = "No se ha podido eliminar el sensor.";
                    claseModal = modalError;
                }
            }
            this.eliminado = true;
            this.StateHasChanged(); //el componente debe refrescarse para mostrar la vista sin el proyecto           
        }

        //protected async Task EliminarProyecto(int idProyecto)
        //{
        //    Console.WriteLine("Función 'eliminar proyecto' activada.");

        //    var resultadoBorrado = await servicioProyecto.EliminarProyecto(idProyecto);
        //    this.eliminado = true;

        //    if (resultadoBorrado)
        //        mensajeEliminar = "¡¡ PROYECTO ELIMINADO !!";
        //    else
        //        mensajeEliminar = " ERROR AL ELIMINAR EL PROYECTO";

        //    this.StateHasChanged(); //el componente debe refrescarse para mostrar la vista sin el proyecto
        //}
        //protected async Task EliminarEstacionBase(int idEstacionBase)
        //{
        //    Console.WriteLine("Función 'EliminarEstacionBase' activada.");

        //    var resultadoBorrado = await servicioEstacionBase.EliminarEstacionBase(idEstacionBase);
        //    this.eliminado = true;

        //    if (resultadoBorrado)
        //        mensajeEliminar = "¡¡ ESTACIÓN ELIMINADA !!";
        //    else
        //        mensajeEliminar = " ERROR AL ELIMINAR LA ESTACIÓN BASE";

        //    this.StateHasChanged(); //el componente debe refrescarse para mostrar la vista sin el proyecto
        //    await CargarDatos();
        //}

        protected async Task Close()
        {
            this.crear = false;
            this.creado = false;

            this.editar = false;
            this.editado = false;

            this.eliminar = false;
            this.eliminado = false;

            //this.claseModal = "";
            this.EstacionBaseEditar = null;

            await CargarDatos();
            this.StateHasChanged();
        }

        protected async Task CargarDatos()
        {          
            proyectos = await servicioProyecto.ObtenerProyectos(idUsuario);
            estaciones = new List<EntidadEstacionBase>();

            if (proyectos != null)
            {
                foreach (var proyecto in proyectos)
                {
                    var estacionesProyecto = await servicioEstacionBase.ListaEstacionesBase(proyecto.Nombre);
                    foreach (var estacionProyecto in estacionesProyecto)
                    {
                        estaciones.Add(estacionProyecto);
                    }
                }
                sensores = new List<EntidadSensorResultado>();
                foreach (var estacion in estaciones)
                {
                    var sensoresEstacion = await servicioEstacionBase.ObtenerSensores(estacion.Nombre);
                    foreach (var sensorEstacion in sensoresEstacion)
                    {
                        sensores.Add(sensorEstacion);
                    }
                }
            }

        }
    }
      
}
