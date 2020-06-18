using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
//using PortalWebLogin.Pages;
using PortalWebLogin.Data;
using PortalWebLogin.Model;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortalWebLogin.ViewModel
{
    public class AdministrarProyectosViewModel : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var usuario = (await authenticationStateTask).User;

            if (!usuario.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("Identity/Account/Login");
            }
        }

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

        private ModeloSensor sensor = null;
        protected ModeloSensor SensorEditar
        {
            get
            {
                return sensor;
            }
            set
            {
                sensor = value;
                this.StateHasChanged();
            }
        }

        private ModeloProyecto proyecto = null;
        protected ModeloProyecto ProyectoEditar
        {
            get
            {
                return proyecto;
            }

            set
            {
                proyecto = value;
                this.StateHasChanged();
            }
        }

        protected ModeloProyecto Proyecto;
        protected ModeloEstacionBase EstacionBase; //atributo para crear una estacion base
        protected ModeloSensor Sensor; //atributo para crear un sensor

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

        protected string mensajeEditar;

        protected IEnumerable<EntidadProyecto> proyectos = null; //lista de todos los proyectos del usuario
        protected List<EntidadEstacionBase> estaciones;
        protected List<EntidadSensorResultado> sensores;

        public bool creado = false; //indica si se ha creado o no el proyecto
        public bool crear, crear_estacionbase, crear_sensor = false;  //indica si se va a crear un proyecto o no

        public bool editar, editado = false; //indican si se va a editar un proyecto y si se ha editado

        public bool eliminar, eliminado = false; //indica si se ha pulsado el boton eliminar y si se ha eliminado un proyecto
        
        public int idEliminar; //id del elemento (proyecto, estacion o sensor) que se va a eliminar
        protected EntidadTratada entidadEliminar;

        protected bool editarProyecto, editarEstacionBase, editarSensor = false;

        // > -- SERVICIOS
        private IServicioProyecto servicioProyecto = FactoriaServicios.GetServicioProyecto();
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();
        private IServicioSensor servicioSensor = FactoriaServicios.GetServicioSensor();

        private int idUsuario; // usuario logado

        protected override async Task OnInitializedAsync()
        {           
            idUsuario = InformacionUsuario.IdUsuario; //ide del usuario -> BORRAR            

            await CargarDatos();
            
            this.StateHasChanged();
        }

        protected async Task Crear()
        {
            //Console.WriteLine("Función crear activada.");
            Proyecto = new ModeloProyecto();
            this.crear = true;
        }
        public async Task CrearProyecto()
        {
            //Console.WriteLine("Función crear proyecto activada.");
            //servicioProyecto = new ServicioProyecto(cadenaConexion, null);

           await servicioProyecto.Crear(new EntidadProyecto()
            {
                Nombre = Proyecto.Nombre,
                Descripcion = Proyecto.Descripcion,
            }, idUsuario);

            creado = true;
            this.StateHasChanged();
        }

        protected async Task CrearEB(int proyecto)
        {
            //Console.WriteLine("Función crear estacion base activada.");
            EstacionBase = new ModeloEstacionBase();
            EstacionBase.FK_IdProyecto = proyecto;
           
            this.crear_estacionbase = true;
        }

        public async Task CrearEstacionBase()
        {

            //Console.WriteLine("Función crear estación base activada.");
           // servicioEstacionBase = new ServicioEstacionBase(cadenaConexion, null);

            await servicioEstacionBase.Crear(new EntidadEstacionBase()
            {
                Nombre = EstacionBase.Nombre,
                FK_IdProyecto = EstacionBase.FK_IdProyecto            

            });


            creado = true;
            this.StateHasChanged();
        }

        protected async Task CrearSE(int idEstacionBase)
        {
            //Console.WriteLine("Función activar sensor activada.");
            Sensor = new ModeloSensor();
            Sensor.FK_IdEstacionBase = idEstacionBase;
            

            this.crear_sensor = true;
        }
        public async Task CrearSensor()
        {
            //Console.WriteLine("Función crear sensor activada.");

            // -- comprobamos si se ha introducido alguna coordenada con punto y lo cambiamos por una coma para que se represente bien
            // -- IMPORTANTE > las coordenadas se representan con ,
            var longitud = Sensor.Longitud.ToString();
            var latitud = Sensor.Latitud.ToString();
            if (longitud.Contains('.')) 
            {
                longitud.Replace('.', ',');
            }
            if (latitud.Contains('.'))
            {
                latitud.Replace('.', ',');
            }

            await servicioSensor.Crear(new EntidadSensor()
            {
                Nombre = Sensor.Nombre,                
                Longitud = longitud,
                Latitud = latitud,
                FK_IdEstacionBase = Sensor.FK_IdEstacionBase
            });

            creado = true;
            this.StateHasChanged();
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
            bool resultado = await servicioEstacionBase.Editar(estacion);
            
            if (resultado)
            {
                mensajeEditar = "Estación editada con éxito.";
            }
            else
            {
                mensajeEditar = "No se ha podido editar la estación base";
            }

            this.editado = true;
            this.StateHasChanged();
        }

        protected void ActivarEditar(EntidadSensorResultado s)
        {
            double longitud, latitud;
            Double.TryParse(s.Longitud, out longitud);
            Double.TryParse(s.Latitud, out latitud);

            SensorEditar = new ModeloSensor()
            {
                Id = s.IdSensor,
                Nombre = s.NombreSensor,
                Longitud = longitud,
                Latitud = latitud
            };
        }
        protected async Task EditarSensor()
        {
            var longitud = SensorEditar.Longitud.ToString();
            var latitud = SensorEditar.Latitud.ToString();
            if (longitud.Contains('.'))
            {
                longitud.Replace('.', ',');
            }
            if (latitud.Contains('.'))
            {
                latitud.Replace('.', ',');
            }

            var sensor = new EntidadSensor()
            {               
                Id = SensorEditar.Id,
                Nombre = SensorEditar.Nombre,
                Latitud = latitud,
                Longitud = longitud
            };

            bool resultado = await servicioSensor.Editar(sensor);
            
            if (resultado)
            {
                mensajeEditar = "El sensor se ha modificado con éxito.";
            }
            else
            {
                mensajeEditar = "No se ha podido editar el sensor.";
            }

            this.editado = true;
            this.StateHasChanged();
        }

        protected void ActivarEditar(EntidadProyecto p)
        {
            ProyectoEditar = new ModeloProyecto()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion
            };
        }
        protected async Task EditarProyecto()
        {
            var proyecto = new EntidadProyecto()
            {
                Id = ProyectoEditar.Id,
                Nombre = ProyectoEditar.Nombre,
                Descripcion = ProyectoEditar.Descripcion

            };
           

            bool resultado = await servicioProyecto.Editar(proyecto);

            if (resultado)
            {
                mensajeEditar = "Los cambios se han guardado con éxito.";
            }
            else
            {
                mensajeEditar = "No se ha podido editar el proyecto.";
            }

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
                resultadoBorrado = await servicioProyecto.Eliminar(idEliminar);
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
                resultadoBorrado = await servicioEstacionBase.Eliminar(idEliminar);

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

        //protected async Task Eliminar(int idProyecto)
        //{
        //    Console.WriteLine("Función 'eliminar proyecto' activada.");

        //    var resultadoBorrado = await servicioProyecto.Eliminar(idProyecto);
        //    this.eliminado = true;

        //    if (resultadoBorrado)
        //        mensajeEliminar = "¡¡ PROYECTO ELIMINADO !!";
        //    else
        //        mensajeEliminar = " ERROR AL ELIMINAR EL PROYECTO";

        //    this.StateHasChanged(); //el componente debe refrescarse para mostrar la vista sin el proyecto
        //}
        //protected async Task Eliminar(int idEstacionBase)
        //{
        //    Console.WriteLine("Función 'Eliminar' activada.");

        //    var resultadoBorrado = await servicioEstacionBase.Eliminar(idEstacionBase);
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
            this.SensorEditar = null;
            this.ProyectoEditar = null;

            this.crear_estacionbase = false;
            this.crear_sensor = false;
            

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
                    var estacionesProyecto = await servicioEstacionBase.ListaEstacionesBase(proyecto.Id);
                    foreach (var estacionProyecto in estacionesProyecto)
                    {
                        estaciones.Add(estacionProyecto);
                    }
                }
                sensores = new List<EntidadSensorResultado>();
                foreach (var estacion in estaciones)
                {
                    var sensoresEstacion = await servicioEstacionBase.ObtenerSensores(estacion.Id);
                    foreach (var sensorEstacion in sensoresEstacion)
                    {
                        sensores.Add(sensorEstacion);
                    }
                }
            }

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
      
}
