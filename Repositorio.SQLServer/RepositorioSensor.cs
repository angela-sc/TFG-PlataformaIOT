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
            bool insertado;
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
                insertado = true;
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (InsertaSensor) - {ex.Message}");
                insertado = false; //si sucede algo devuelve false
            }
            return insertado;
        }

        public async Task<bool> InsertaDato(EntidadDato dato)
        {
            bool insertado;
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@stamp", dato.Stamp },
                { "@fk_idsensor", dato.FK_IdSensor },
                { "@humedad", dato.Humedad },
                { "@temperatura", dato.Temperatura }
            };

            //query sql para insertar los datos en la tabla
            string query = @"INSERT INTO [plataforma_iot].[dbo].[Datos] ([stamp],[fk_idsensor],[humedad],[temperatura]) 
                             VALUES (@stamp, @fk_idsensor, @humedad, @temperatura)";

            try
            {               
                using (SqlConnection conn = new SqlConnection(cadenaConexion))  //nueva conexion con la BD. Al utilizar using nos aseguramos de que se libera la sesion
                {
                    await conn.ExecuteAsync(query, parametros);
                    insertado = true;
                }               
            }
            catch(Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (InsertaDato) - {ex.Message}");
                insertado = false;
            }
            return insertado;          
        }

        
        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top) // > -- Obtiene los datos para un sensor determinado. Se utiliza en la vista detallada de sensor
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idSensor },
                { "@top", top }
            };

            string query = @"SELECT top (@top) [stamp] [Stamp], [humedad] [Humedad], [temperatura] [Temperatura] 
                             FROM [plataforma_iot].[dbo].[Datos] WHERE [fk_idsensor] = @id";
            
            IEnumerable<EntidadDatoBase> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    result = await conn.QueryAsync<EntidadDatoBase>(query, parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (ObtenerDatos) - {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, DateTime? fechaInicio, DateTime? fechaFin) // > -- filtra los datos para un sensor determinado según una fecha de inicio y fin
        {

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idSensor },
                { "@fechainicio", fechaInicio },
                { "@fechafin", fechaFin }
            };

            string query = @"SELECT [stamp] [Stamp], [humedad] [Humedad], [temperatura] [Temperatura] 
                             FROM [plataforma_iot].[dbo].[Datos] WHERE [fk_idsensor] = @id AND [stamp] BETWEEN @fechainicio AND @fechafin";
            
            IEnumerable<EntidadDatoBase> result = null;

            if (fechaInicio != null && fechaFin != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cadenaConexion))
                    {
                        result = await conn.QueryAsync<EntidadDatoBase>(query, parametros);
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"ERR.REPOSITORIO SENSOR (ObtenerDatos) - {ex.Message}");
                }               
            }
            else
            {
                result = null;
                if (fechaFin == null)
                {
                    log.Error("fecha de fin vacía - ERR. REPOSITORIO SENSOR (ObtenerDatos)");
                }
                if(fechaInicio == null)
                {
                    log.Error("fecha de inicio vacía - ERR. REPOSITORIO SENSOR (ObtenerDatos)");
                }                
            }
            return result;
        }
        
        public async Task<int> ObtenerId(string nombreSensor, int idEstacionBase)
        {
            int id = -1;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {             
                { "@fk_idestacionbase", idEstacionBase },
                { "@nombresensor", nombreSensor }
            };

            string query = @"SELECT [id] FROM [plataforma_iot].[dbo].[Sensor] WHERE [fk_idestacionbase] = @fk_idestacionbase AND [nombre] = @nombresensor";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    var resultado = await conn.QueryAsync<int>(query, parametros);
                    id = resultado.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (ObtenerId) - {ex.Message}");
                id = -1 ;
            }
            return id;
        }

        public async Task<string> ObtenerNombre(int idSensor, int idEstacionBase)
        {
            string nombre = null;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@idSensor", idSensor },
                { "@fk_idestacionbase", idEstacionBase }                
            };

            string query = @"SELECT [nombre] FROM [plataforma_iot].[dbo].[Sensor] WHERE [id] = @idSensor AND [fk_idestacionbase] = @fk_idestacionbase";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    var resultado = await conn.QueryAsync<string>(query, parametros);
                    nombre = resultado.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (ObtenerNombre) - {ex.Message}");
                nombre = null;
            }
            return nombre;
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
                eliminado = true;

            }catch(Exception ex)
            {
                log.Error($"ERR. REPOSITORIO SENSOR (EliminarDatos) - {ex.Message}");
                return eliminado; //¿esto va aqui?
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
                log.Error($"ERR. REPOSITORIO SENSOR (Eliminarsensor) - {ex.Message}");               
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
                { "@nombre", sensor.Nombre},
                { "@latitud", sensor.Latitud},
                { "@longitud", sensor.Longitud}
            };

            string query = @"UPDATE [plataforma_iot].[dbo].[Sensor]
                             SET [nombre] = @nombre, [latitud] = @latitud, [longitud] = @longitud
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
                log.Error($"ERR. REPOSITORIO SENSOR (Editar) - {ex.Message}");
                editado = false;
            }

            return editado;
        }  
    }
}
