using Libreria.Interfaces;
using Serilog;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstacionBase.WorkerService
{
    public static class FactoriaServicios
    {
        
        private static IServicioSeguridad servicioSeguridad;

        public static string FicheroClaveRSA { get; set; }
        public static string DirectorioSensores { get; set; }
        public static Uri UriCOAP { get; set; }
        public static ILogger Log { get; set; }
        public static string Proyecto { get; set; }
        public static string EstacionBase { get; set; }



        public static IServicioSeguridad GetServicioSeguridad()
        {
            if (Log == null)
            {
                throw new ArgumentNullException("Log - {appsettings.json}");
            }
            else if (string.IsNullOrEmpty(FicheroClaveRSA))
            {
                throw new ArgumentNullException("FicheroClaveRSA -{appsettings.json}");
            }
            else
            {
                if (servicioSeguridad == null)
                {
                    servicioSeguridad = new ServicioSeguridad(FicheroClaveRSA, Log);
                }

                return servicioSeguridad;
            }

        }
    }
}
