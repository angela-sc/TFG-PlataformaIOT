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
        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top)
        {
            IEnumerable<EntidadDatoBase> datos = null;

            try
            {
                datos = await repositorioSensor.ObtenerDatos(idSensor, top); 
            }
            catch (Exception)
            {
                log.Warning($"Problema al obtener datos del sensor {idSensor}; no se encuentra en la base de datos.");
            }
            
            return datos; //devuelve null o un IEnumerable<EntidadDatoBase>
        }

        //Devuelve una lista con los datos de temperatura de un sensor determinado
        public async Task<IEnumerable<double>> ObtenerTemperatura(int idSensor, int top)
        {
            IEnumerable<EntidadDatoBase> AllData = await repositorioSensor.ObtenerDatos(idSensor, top);
            List<double> temperatura = new List<double>();

            foreach(EntidadDatoBase dato in AllData)
            {
                temperatura.Add((double)dato.Temperatura);
            }

            return temperatura;
        }  

        public async Task<int> ObtenerIdSensor(string nombreSensor, string nombreEstacionBase)
        {
            int resultado = -1;
            try
            {
                resultado = await repositorioSensor.ObtenerId(nombreSensor, 2);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public async Task<bool> EliminarDatos(int fk_SensorId)
        {
            return await repositorioSensor.EliminarDatos(fk_SensorId);
        }

        public async Task<bool> EliminarSensor(int sensorid)
        {
            return await repositorioSensor.EliminarSensor(sensorid);
        }
    }
}
