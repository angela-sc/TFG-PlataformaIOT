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
                datos = await repositorioSensor.GetData(idSensor, top); 
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

        //Devuelve una lista con los datos de temperatura de un sensor determinado
        public async Task<IEnumerable<double>> ObtenerTemperatura(int idSensor, int top)
        {
            IEnumerable<EntidadDatoBase> AllData = await repositorioSensor.GetData(idSensor, top);
            List<double> temperatura = new List<double>();

            foreach(EntidadDatoBase dato in AllData)
            {
                temperatura.Add((double)dato.temperature);
            }

            return temperatura;
        }  

        public async Task<int> ObtenerIdSensor(string nombreSensor, string nombreEstacionBase)
        {
            int resultado = -1;
            try
            {
                resultado = await repositorioSensor.GetId(nombreSensor, 2);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public async Task<bool> EliminarDatos(int fk_SensorId)
        {
            bool eliminado = false;
            //try
            //{


            eliminado = await repositorioSensor.EliminarDatos(fk_SensorId);
            // eliminado = true;

            //}catch(Exception ex)
            //{
            //    Console.WriteLine("Error el eliminar los datos: ", ex.Message);
            //    return eliminado;
            //}

            return eliminado;
        }

        public async Task<bool> EliminarSensor(int fk_estacionbaseid, int sensorid)
        {
            bool eliminado = false;
            //try
            //{
            //if(await repositorioSensor.EliminarDatos(sensorid))
            //{


            //}
            eliminado = await repositorioSensor.EliminarSensor(fk_estacionbaseid, sensorid);

            // eliminado = true;

            //}catch(Exception ex)
            //{
            //    Console.WriteLine("Error el eliminar los datos: ", ex.Message);
            //    return eliminado;
            //}

            return eliminado;
        }
    }
}
