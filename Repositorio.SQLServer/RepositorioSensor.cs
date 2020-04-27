using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.SQLServer
{
    public class RepositorioSensor : IRepositorioSensor
    {
        private string connectionString;
        private ILogger log;

        public RepositorioSensor(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.log = logger;
        }

       

        public async Task<bool> InsertaSensor(EntidadSensor sensor)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@name", sensor.Name },
                {"@longitud", sensor.Longitud },
                {"@latitud", sensor.Latitud },
                { "@fk_basestationid", sensor.FK_basestationID }
            };

            string query = @"INSERT INTO [plataformadb].[dbo].[Sensor] ([Name],[Location],[FK_BaseStationId])
                                VALUES (@name, @location, @fk_basestationid)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.ExecuteAsync(query, queryParams);
                }
            }
            catch (Exception)
            {
                //throw ex; //excepcion al establecer la conexion o al ejecutar el async
                //Console.WriteLine(ex.Message, "Error en RepositorioSensor en el metodo InsertaSensor");
                log.Error($"Error al insertar el sensor {sensor.Name} en la base de datos");
                return false; //si sucede algo, directamente devuelve false
            }
            return true;
        }

        public async Task<bool> InsertaDato(EntidadDato dato)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@stamp", dato.stamp },
                { "@fk_sensor", dato.FK_sensorID },
                { "@humity", dato.humity },
                { "@temperature", dato.temperature }
            };

            //query sql para insertar los datos en la tabla
            string query = @"INSERT INTO [plataformadb].[dbo].[Data] ([Stamp],[FK_SensorId],[humity],[temperature]) 
                             VALUES (@stamp,@fk_sensor,@humity,@temperature)";

            try
            {
                //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.ExecuteAsync(query, queryParams);
                }
            }
            catch(Exception)
            {
                //Console.WriteLine("Error en el método InsertaDato " + ex.Message);
                log.Error($"No se ha podido insertar el dato {dato.stamp} en el sensor.");
                return false;
            }

            return true;          
        }

        public async Task<IEnumerable<EntidadDatoBase>> GetData(int idSensor, int top)
        {
            string query = String.Format(@"SELECT top ({0}) [Stamp] as [stamp], [humity], [temperature] FROM [plataformadb].[dbo].[Data] WHERE [FK_SensorId] = {1}", top, idSensor);
            IEnumerable<EntidadDatoBase> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    result = await conn.QueryAsync<EntidadDatoBase>(query);
                }
            }
            catch (Exception)
            {
                log.Warning($"No se ha encontrado el sensor {idSensor} en la base de datos.");
            }

            return result;
        }

        public async Task<int> GetId(string nombreSensor, int idEstacionBase)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@sensor", nombreSensor },
                { "@fk_estacionbase", idEstacionBase }
            };

            string query = String.Format(@"SELECT [Id] FROM [plataformadb].[dbo].[Sensor] WHERE [Name]= @sensor AND [FK_BaseStationId] = @fk_estacionbase");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var result = await conn.QueryAsync<int>(query, queryParams);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                //throw ex; //excepcion al establecer la conexion
                //Console.WriteLine(ex.Message, "Error en RepositorioSensor en el metodo GetID");
                log.Warning($"No se ha encontrado ningun id para el sensor {nombreSensor} en la estacion base {idEstacionBase}");
                return -1;
            }
        }

        
    }
}
