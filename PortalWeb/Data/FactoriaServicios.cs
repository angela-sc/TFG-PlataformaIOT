using Libreria.Interfaces;
using Serilog;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.Data
{
    public static class FactoriaServicios
    {
        private static string cadenaConexion;
        private static ILogger log;
        private static IServicioProyecto servicioProyecto = null;
        private static IServicioEstacionBase servicioEstacionBase = null;
        private static IServicioSensor servicioSensor = null;

        public static void SetCadenaConexion(string conexion)
        {
            cadenaConexion = conexion;
        }

        public static void SetLogger(ILogger logger)
        {
            log = logger;
        }        
        
        public static IServicioProyecto GetServicioProyecto()
        {
            if(cadenaConexion == null)
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
                    servicioProyecto = new ServicioProyecto(cadenaConexion, log);
                }

                return servicioProyecto;
            }            
        }
        public static IServicioEstacionBase GetServicioEstacionBase()
        {
            if (cadenaConexion == null)
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
                    servicioEstacionBase = new ServicioEstacionBase(cadenaConexion, log);                    
                }

                return servicioEstacionBase;
            }
        }

        public static IServicioSensor GetServicioSensor()
        {
            if (cadenaConexion == null)
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
                    servicioSensor = new ServicioSensor(cadenaConexion, log);
                }

                return servicioSensor;
            }            
        }
    }
}
