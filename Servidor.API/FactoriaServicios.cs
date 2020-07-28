using System;
using System.Collections.Generic;
using System.Text;
using Libreria.Interfaces;
using Serilog;
using Servicios;

namespace Servidor.API
{
    public static class FactoriaServicios
    {
        public static Serilog.ILogger Log { get; set; }
        public static string CadenaConexion { get; set; }
        public static string FicheroClaveRSA { get; set; }
        public static string Puerto { get; set; }

        private static IServicioInsertaInformacion servicioInsertaInformacion;
        private static IServicioSeguridad servicioSeguridad;

        public static IServicioInsertaInformacion GetServicioInsertaInformacion()
        {
            if (CadenaConexion == null)
            {
                throw new ArgumentNullException("CadenaConexion - {appsettings.json}");
            }
            else if (Log == null)
            {
                throw new ArgumentNullException("Log - {appsettings.json}");
            }
            else
            {
                if(servicioInsertaInformacion == null)
                    servicioInsertaInformacion = new ServicioInsertaInformacion(CadenaConexion, Log);

                return servicioInsertaInformacion;
            }
        }

        public static IServicioSeguridad GetServicioSeguridad()
        {
            if (FicheroClaveRSA == null)
                throw new ArgumentNullException("FicheroClaveRSA - {appsettings.json}");
            if (Log == null)
                throw new ArgumentNullException("Log - {appsettings.json}");

            if(servicioSeguridad == null)
                servicioSeguridad = new ServicioSeguridad(FicheroClaveRSA, Log);

            return servicioSeguridad;
        }
    }
}
