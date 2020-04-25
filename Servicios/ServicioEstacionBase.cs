using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioEstacionBase : IServicioEstacionBase
    {
        private IRepositorioEstacionBase repositorioEstacionBase;

        private string cadenaConexion;
        private ILogger log;

        public ServicioEstacionBase(string cadenaConexion, ILogger log)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = log;

            this.repositorioEstacionBase = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase)
        {
            IEnumerable<EntidadSensorResultado> sensores = new List<EntidadSensorResultado>();
            sensores = await repositorioEstacionBase.ObtenerSensores(nombreEstacionBase);

            return sensores;
        }

        //public async Task<IEnumerable<EntidadMarcadorMapa>> ObtenerCoordenadasSensores(string nombreEstacionBase)
        //{
        //    IEnumerable<EntidadCoordenada> coordenadas = new List<EntidadCoordenada>();
        //    coordenadas = await repositorioEstacionBase.ObtenerCoordenadasSensores(nombreEstacionBase);
            
        //    List<EntidadMarcadorMapa> resultado = new List<EntidadMarcadorMapa>();
        //    foreach(EntidadCoordenada coord in coordenadas)
        //    {
        //        double aux_latitud = Convert.ToDouble(coord.latitud);
        //        double aux_longitud = Convert.ToDouble(coord.longitud);
        //        resultado.Add(new EntidadMarcadorMapa(coord.nombreSensor, aux_latitud, aux_longitud));
        //    }

        //    return resultado;
        //}

        //Metodo para obtener la lista de estaciones base pertenecientes a un proyecto > ¿pasamos id o nombre del proyeecto?
        public async Task<IEnumerable<EntidadEstacionBase>> ListaEstacionesBase(string nombreProyecto)
        {
            IEnumerable<EntidadEstacionBase> estacionesBase = new List<EntidadEstacionBase>();
            try
            {
                estacionesBase = await repositorioEstacionBase.ObtenerEstacionesBase(nombreProyecto);

            }
            catch (Exception)
            {
                estacionesBase = null;

            }

            return estacionesBase;
        }
    }
}
