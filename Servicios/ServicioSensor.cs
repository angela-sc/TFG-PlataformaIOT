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
        
        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top) //Obtiene todos los datos asociados a un sensor, sin filtros
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

        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, DateTime? fechaInicio, DateTime? fechaFin) //Obtiene todos los datos asociados a un sensor, FILTRANDO por fecha de inicio y fin
        {
            IEnumerable<EntidadDatoBase> datos = null;

            try
            {
                datos = await repositorioSensor.ObtenerDatos(idSensor, fechaInicio, fechaFin);
            }
            catch (Exception)
            {
                log.Warning($"Problema al obtener datos del sensor {idSensor}; no se encuentra en la base de datos.");
            }

            return datos; //devuelve null o un IEnumerable<EntidadDatoBase>
        }


        public async Task<IEnumerable<double>> ObtenerTemperatura(int idSensor, int top) //Devuelve una lista con los datos de temperatura de un sensor determinado
        {
            IEnumerable<EntidadDatoBase> AllData = await repositorioSensor.ObtenerDatos(idSensor, top);
            List<double> temperatura = new List<double>();

            foreach(EntidadDatoBase dato in AllData)
            {
                temperatura.Add((double)dato.Temperatura);
            }

            return temperatura;
        }   

        public async Task<int> ObtenerId(string nombreSensor, int idEstacionBase)
        {
            int resultado;// = -1;
            try
            {
                resultado = await repositorioSensor.ObtenerId(nombreSensor, idEstacionBase);
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                log.Error($"ERR. SERVICIO SENSOR (ObtenerId) - {ex.Message}");
                resultado = -1;
            }

            return resultado;
        }

        public async Task<string> ObtenerNombre(int idSensor, int idEstacionBase)
        {
            return await repositorioSensor.ObtenerNombre(idSensor, idEstacionBase);
        }

        public async Task<bool> EliminarDatos(int fk_SensorId)
        {
            bool eliminado; // = false;
            try
            {
                eliminado = await repositorioSensor.EliminarDatos(fk_SensorId);
            }catch(Exception ex)
            {
                eliminado = false;
                log.Error($"ERR. SERVICIO SENSOR (EliminarDatos) - {ex.Message}");
            }
            return eliminado; 
        }

        public async Task<bool> EliminarSensor(int sensorid)
        {
            //return await repositorioSensor.EliminarSensor(sensorid);

            bool eliminado;

            try
            {
                eliminado = await repositorioSensor.EliminarSensor(sensorid);
            }
            catch (Exception ex)
            {
                eliminado = false;
                log.Error($"ERR. SERVICIO SENSOR (EliminarSensor) - {ex.Message}");
            }
            return eliminado;
        }

        public async Task<bool> Editar(EntidadSensor sensor)
        {
            //return await repositorioSensor.Editar(sensor);
            bool editado;

            try
            {
                editado = await repositorioSensor.Editar(sensor);
            }
            catch (Exception ex)
            {
                editado = false;
                log.Error($"ERR. SERVICIO SENSOR (Editar) - {ex.Message}");
            }
            return editado;
        }

        public async Task Crear(EntidadSensor sensor)
        {
            await repositorioSensor.InsertaSensor(sensor);
        }
    }
}
