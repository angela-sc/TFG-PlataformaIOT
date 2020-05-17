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
        private string cadenaConexion;
        private ILogger log;

        public RepositorioSensor(string cadenaConexion, ILogger logger)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = logger;
        }

        /**
         * <summary>
         * <example>
         *      <code>
         *           await repoSensor.InsertaSensor(entidadSensor)
         *      </code>
         * </example>
         * <param name="sensor">Datos del sensor a insertar</param>
         * <returns>Task<bool></returns>
         * </summary>
         * */
        public async Task<bool> InsertaSensor(EntidadSensor sensor)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@nombre", sensor.Nombre },
                { "@longitud", sensor.Longitud },
                { "@latitud", sensor.Latitud },
                { "@fk_idestacionbase", sensor.FK_IdEstacionBase }
            };

            string query = @"INSERT INTO [plataforma_iot].[dbo].[Sensor] ([nombre],[longitud],[latitud],[fk_idestacionbase])
                                VALUES (@nombre, @longitud, @latitud, @fk_idestacionbase)";
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                }
            }
            catch (Exception)
            {
                log.Error($"Ha habido un problema al insertar el sensor {sensor.Nombre} en la base de datos - ERR. REPOSITORIO SENSOR");
                return false; //si sucede algo, directamente devuelve false
            }
            return true;
        }

        public async Task<bool> InsertaDato(EntidadDato dato)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@stamp", dato.Stamp },
                { "@fk_idsensor", dato.FK_IdSensor },
                { "@humedad", dato.Humedad },
                { "@temperatura", dato.Temperatura }
            };

            //query sql para insertar los datos en la tabla
            string query = @"INSERT INTO [plataforma_iot].[dbo].[Datos] ([stamp],[fk_idsensor],[humedad],[temperatura]) 
                             VALUES (@stamp, @fk_sensor, @humedad, @temperatura)";
            try
            {               
                using (SqlConnection conn = new SqlConnection(cadenaConexion))  //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
                {
                    await conn.ExecuteAsync(query, parametros);
                }
            }
            catch(Exception)
            {
                log.Error($"No se ha podido insertar el dato {dato.Stamp} en el sensor - ERR. REPOSITORIO SENSOR");
                return false;
            }
            return true;          
        }

        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idSensor},
                { "@top", top }
            };

            string query = @"SELECT top (@top) [stamp] [Stamp], [humedad] [Humedad], [temperatura] [Temperatura] 
                             FROM [plataforma_iot].[dbo].[Datos] WHERE [fk_idsensor] = @id";
            
            IEnumerable<EntidadDatoBase> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    result = await conn.QueryAsync<EntidadDatoBase>(query);
                }
            }
            catch (Exception)
            {
                log.Warning($"No se ha encontrado el sensor {idSensor} en la base de datos - ERR. REPOSITORIO SENSOR");
            }
            return result;
        }

        public async Task<int> ObtenerId(string nombreSensor, int idEstacionBase)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@sensor", nombreSensor },
                { "@fk_idestacionbase", idEstacionBase }
            };

            string query = @"SELECT [Id] FROM [plataforma_iot].[dbo].[Sensor] WHERE [Name]= @sensor AND [FK_BaseStationId] = @fk_idestacionbase";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    var result = await conn.QueryAsync<int>(query, parametros);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                //throw ex; //excepcion al establecer la conexion
                //Console.WriteLine(ex.Message, "Error en RepositorioSensor en el metodo GetID");
                log.Warning($"No se ha encontrado ningún id para el sensor {nombreSensor} en la estación base {idEstacionBase} - ERR. REPOSITORIO SENSOR");
                return -1;
            }
        }

        public async Task<bool> EliminarDatos(int fk_idsensor)
        {
            bool eliminado = false;
            
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@fk_idsensor", fk_idsensor }
            };
            string query = string.Format(@"DELETE FROM [plataforma_iot].[dbo].[Datos] WHERE [fk_idsensor] = @fk_idsensor");

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                }

            }catch(Exception ex)
            {
                //log.Error($"No se ha podido eliminar los datos del sensor {fk_idsensor} - ERR. REPOSITORIO SENSOR");
                Console.WriteLine($"Error al borrar los datos del sensor {fk_idsensor}: ", ex.Message);

                return eliminado;
            }
            return eliminado;
        }

        public async Task<bool> EliminarSensor(int idSensor)
        {
            bool eliminado = false;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idSensor }
            };

            string query = @"DELETE FROM [plataforma_iot].[dbo].[Sensor] WHERE [id] = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                    eliminado = true;
                }
            }
            catch (Exception ex)
            {
                //log.Error($"No se ha podido eliminar el sensor {idSensor} - ERR. REPOSITORIO SENSOR");
                Console.WriteLine($"Error al borrar los datos del sensor {idSensor}: ", ex.Message);

                eliminado = false;
            }

            return eliminado;
        }

        public async Task<bool> Editar(EntidadSensor sensor)
        {
            bool editado;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", sensor.Id },
                { "@nombre", sensor.Nombre}
            };

            string query = @"UPDATE [plataforma_iot].[dbo].[Sensor]
                             SET [nombre] = @nombre
                             WHERE [id] = @id ";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                    editado = true;
                }
            }
            catch (Exception ex)
            {
                //log.Error($"Se ha producido un error al eliminar el proyecto - ERR. REPOSITORIO SENSOR");
                Console.WriteLine(ex.Message, "Error: ");
                editado = false;
            }

            return editado;
        }
        
    }
}
