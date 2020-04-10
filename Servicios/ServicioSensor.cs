using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{   
    public class ServicioSensor : IServicioSensor
    {
        private ILogger log;
        private IRepositorioSensor repositorioSensor;

        public ServicioSensor(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioSensor = new RepositorioSensor(cadenaConexion, log);
        }

        

        //Obtiene todos los datos asociados a un sensor, sin filtros
        public async Task<List<EntidadDatoBase>> ObtenerDatos(int idSensor)
        {
            List<EntidadDatoBase> datos = null; 

            try
            {
                datos = await repositorioSensor.GetData(idSensor); 
            }
            catch (Exception)
            {
                log.Warning($"Problema al obtener datos del sensor {idSensor}; no se encuentra en la base de datos.");
            }
            
            return datos; //devuelve null o un IEnumerable<EntidadDatoBase>
        }

        //Obtiene los datos asociados a un sensor FILTRANDO por fechas
        //public async Task<List<EntidadDatoBase>> BuscarDatos(int idSensor, DateTime inicio, DateTime fin)
        //{
        //    List<EntidadDatoBase> datos = null;
        //}
    }
}
