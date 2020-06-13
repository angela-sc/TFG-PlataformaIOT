using Libreria.Interfaces;
using Serilog;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public static class FactoriaServicios
    {
        public static ILogger Log { get; set; }
        public static string CadenaConexion { get; set; }
        public static string FicheroClaveRSA { get; set; }

        private static IServicioInsertaInformacion servicioInsertaInformacion;
        private static IServicioSeguridad servicioSeguridad;

        public static IServicioInsertaInformacion GetServicioInsertaInformacion()
        {
            if (CadenaConexion == null)
            {
                throw new ArgumentNullException("Cadena de conexión vacía");
            }
            else if (Log == null)
            {
                throw new ArgumentNullException("Log vacío");
            }
            else
            {
                if (servicioInsertaInformacion == null)
                {
                    servicioInsertaInformacion = new ServicioInsertaInformacion(CadenaConexion, Log);
                }

                return servicioInsertaInformacion;
            }
        }

        public static IServicioSeguridad GetServicioSeguridad()
        {
            if(FicheroClaveRSA == null)
                throw new ArgumentNullException("FicheroClavesRSA no existe.");
            if (Log == null)
                throw new ArgumentNullException("Log vacío.");

           
            if (servicioSeguridad == null)
            {
                servicioSeguridad = new ServicioSeguridad(FicheroClaveRSA, Log);
            }

            return servicioSeguridad;            
        }
    }
}
