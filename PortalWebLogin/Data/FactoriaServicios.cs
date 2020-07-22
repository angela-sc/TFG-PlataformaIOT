using Libreria.Interfaces;
using Serilog;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebLogin.Data
{
    public static class FactoriaServicios
    {
        public static string CadenaConexion { get; set; }
        private static ILogger log;
        private static IServicioProyecto servicioProyecto = null;
        private static IServicioEstacionBase servicioEstacionBase = null;
        private static IServicioSensor servicioSensor = null;
        private static string directorioTemporal; // >- Directorios donde se guardan  las claves rsa


        public static void SetLogger(ILogger logger)
        {
            log = logger;
        }      

        public static void SetDirectorioTemporal(string directorio)
        {
            directorioTemporal = string.IsNullOrEmpty(directorio) ? throw new ArgumentNullException("DirectorioTemporal (appsettings.json)") : directorio;            
        }

        public static string GetDirectorioTemporal()
        {
            return string.IsNullOrEmpty(directorioTemporal) ? throw new ArgumentNullException("DirectorioTemporal (appsettings.json)") : directorioTemporal;
        }


        public static IServicioProyecto GetServicioProyecto()
        {
            if(CadenaConexion == null)
            {
                throw new ArgumentNullException("Cadena de conexión vacía");
            }
            else if(log == null)
            {
                throw new ArgumentNullException("Log vacío");
            }
            else
            {
                if (servicioProyecto == null)
                {
                    servicioProyecto = new ServicioProyecto(CadenaConexion, log);
                }

                return servicioProyecto;
            }            
        }
        public static IServicioEstacionBase GetServicioEstacionBase()
        {
            if (CadenaConexion == null)
            {
                throw new ArgumentNullException("Cadena de conexión vacía");
            }
            else if (log == null)
            {
                throw new ArgumentNullException("Log vacío");
            }
            else
            {
                if (servicioEstacionBase == null)
                {
                    servicioEstacionBase = new ServicioEstacionBase(CadenaConexion, log);                    
                }

                return servicioEstacionBase;
            }
        }

        public static IServicioSensor GetServicioSensor()
        {
            if (CadenaConexion == null)
            {
                throw new ArgumentNullException("Cadena de conexión vacía");
            }
            else if (log == null)
            {
                throw new ArgumentNullException("Log vacío");
            }
            else
            {
                if (servicioSensor == null)
                {
                    servicioSensor = new ServicioSensor(CadenaConexion, log);
                }

                return servicioSensor;
            }            
        }
    }
}
