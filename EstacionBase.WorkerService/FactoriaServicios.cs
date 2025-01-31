﻿using Libreria.Interfaces;
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

        private static int tiempoEnvio; //cada cuanto tiempo se leen ficheros en el worker
        public static void SetTiempoEnvio(string envio)
        {
            if (string.IsNullOrEmpty(envio))
                throw new ArgumentNullException("TiempoEnvio - {appsettings.json}");

            int envioInt;
            Int32.TryParse(envio, out envioInt);
            tiempoEnvio = envioInt;
            if(tiempoEnvio < 60) // > como mínimo se envía información cada minuto, sino es mucha carga
            {
                tiempoEnvio = 60;
            }
        }
        public static int GetTiempoEnvio()
        {
            return tiempoEnvio;
        }


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
