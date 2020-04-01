using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
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

        public RepositorioSensor(string connectionString)
        {
            this.connectionString = connectionString;
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
            string query = @"INSERT INTO Data([Stamp],[FK_SensorId],[humity],[temperature]) 
                             VALUES (@stamp,@fk_sensor,@humity,@temperature)";

            try
            {
                //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.ExecuteAsync(query, queryParams);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error en el método InsertaDato " + ex.Message);
                return false;
            }

            return true;          
        }

        public void InsertaSensor(EntidadSensor sensor)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@name", sensor.Name },
                { "@location", sensor.Location },
                { "@fk_basestationid", sensor.FK_basestationID }
            };

            string query = @"INSERT INTO [Sensor] ([Name],[Location],[FK_BaseStationId])
                                VALUES (@name, @location, @fk_basestationid)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.ExecuteAsync(query, queryParams);
                }
            }
            catch(Exception ex)
            {
                //throw ex; //excepcion al establecer la conexion o al ejecutar el async
                Console.WriteLine("Error en el método InsertaSensor: " + ex.Message);
            }
        }

        public int GetId(string nombreSensor, int idEstacionBase)
        {
            string query = String.Format("SELECT [Id] FROM [Sensor] WHERE [Name]= '{0}' AND [FK_BaseStationId] = '{1}'", nombreSensor, idEstacionBase);
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var result = conn.QueryAsync<int>(query);
                    return result.Result.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                //throw ex; //excepcion al establecer la conexion
                Console.WriteLine(ex.Message);
                return -1;
            }          
        }
    }
}
