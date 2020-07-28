using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            repositorioEstacionBase = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<string> Nombre (int idEstacionBase)
        {
            string nombre = await repositorioEstacionBase.ObtenerNombre(idEstacionBase);
            return nombre;
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(int idEstacionBase)
        {
            IEnumerable<EntidadSensorResultado> sensores = new List<EntidadSensorResultado>();
            try
            {
                sensores = await repositorioEstacionBase.ObtenerSensores(idEstacionBase);
            }
            catch(Exception ex)
            {
                sensores = null;
                log.Error($"ERR. SERVICIO ESTACION BASE (ObtenerSensores) - {ex.Message}");
            }

            return sensores;
        }
        
        public async Task<IEnumerable<EntidadEstacionBase>> ListaEstacionesBase(int idProyecto) //Metodo para obtener la lista de estaciones base pertenecientes a un proyecto > ¿pasamos id o nombre del proyeecto?
        {
            IEnumerable<EntidadEstacionBase> estacionesBase = new List<EntidadEstacionBase>();
            try
            {
                estacionesBase = await repositorioEstacionBase.ObtenerEstacionesBase(idProyecto);
            }
            catch (Exception ex)
            {
                estacionesBase = null;
                log.Error($"ERR. SERVICIO ESTACION BASE (ListaEstacionesBase) - {ex.Message}");
            }

            return estacionesBase;
        }

        public async Task<bool> Eliminar(int idEstacionBase)
        {
            bool eliminada;
            try
            {
                eliminada = await repositorioEstacionBase.Eliminar(idEstacionBase);
            }
            catch (Exception ex)
            {
                eliminada = false;
                log.Error($"ERR. SERVICIO ESTACION BASE (Eliminar) - {ex.Message}");
            }
            return eliminada;
        }

        public async Task<bool> Editar(EntidadEstacionBase estacionBase)
        {
            bool editado;
            try
            {
                editado = await repositorioEstacionBase.Editar(estacionBase);
            }
            catch(Exception ex)
            {
                editado = false;
                log.Error($"ERR. SERVICIO ESTACION BASE (Editar) - {ex.Message}");
            }
            return editado;
        }

        public async Task Crear(EntidadEstacionBase estacionBase)
        {
            await repositorioEstacionBase.Crear(estacionBase);            
        }

        public async Task<bool> Autorizado(string idUsuario, int idEstacionBase)
        {
            bool autorizado = false;
            try {
                var estacionesBaseUsuario = await repositorioEstacionBase.ObtenerEstacionesBase(idUsuario);
                if (estacionesBaseUsuario != null && estacionesBaseUsuario.Count() > 0)
                    autorizado = estacionesBaseUsuario.Select(_ => _.Id).Contains(idEstacionBase);
            }
            catch(Exception ex)
            {
                log.Error($"ERR. SERVICIO ESTACION BASE (Autorizado) - {ex.Message}");
            }

            return autorizado;
        }
    }
}
