using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Repositorio.SQLServer;
using System.Threading.Tasks;
using Serilog;
using System.Globalization;

namespace Repositorio.SQLServer
{
    public class RepositorioEstacionBase : IRepositorioEstacionBase
    {
        private string cadenaConexion;
        private ILogger log;

        public RepositorioEstacionBase(string cadenaConexion, ILogger logger)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = logger;
        }
      
        public async Task<int> ObtenerId(string nombreProyecto, string nombreEstacionBase) //Metodo que obtiene el id de la estación base a partir de su nombre
        {
            int id = -1;

            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@estacionbase", nombreEstacionBase},
                { "@proyecto", nombreProyecto }
            };

            string query = @"   
                            SELECT eb.[id]
                            FROM [plataforma_iot].[dbo].[EstacionBase] eb
                            JOIN [plataforma_iot].[dbo].[Proyecto] p ON p.[id] = eb.[fk_idproyecto]
                            WHERE p.[nombre] = @proyecto AND eb.[nombre] = @estacionbase
                            ";

            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    var resultado = await con.QueryAsync<int>(query, parametros);
                    id =  resultado.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (ObtenerId) - {ex.Message}");
                id = -1;
            }
            return id;
        }

        public async Task<string> ObtenerNombre(int idEstacionBase)
        {
            string nombre = "";
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@id", idEstacionBase}
            };

            string query = string.Format(@"SELECT [nombre] FROM [plataforma_iot].[dbo].[EstacionBase] WHERE [id] = @id");

            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    var resultado = await con.QueryAsync<string>(query, parametros);
                    nombre = resultado.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (ObtenerNombre) - {ex.Message}");
                nombre = "";
            }
            return nombre;
        }

        public async Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(int idProyecto)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                { "@id", idProyecto }
            };

            string query = @" SELECT eb.* FROM [plataforma_iot].[dbo].[EstacionBase] eb WHERE eb.[fk_idproyecto] = @id";

            IEnumerable<EntidadEstacionBase> estaciones = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    estaciones = await conn.QueryAsync<EntidadEstacionBase>(query, parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (ObtenerEstacionesBase) - {ex.Message}");
            }
            return estaciones;
        }
       
        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(int idEstacionBase)
        {
            /**
                OPCION 1

                SELECT  
	                ls.[Sensor] 
                    ,ls.[Latitud]
                    ,ls.[Longitud]
                    ,d.[Stamp] as [Fecha]
                    ,d.[humity] as [Humedad]
                    ,d.[temperature] as [Temperatura] 
                FROM [plataforma_iot].[dbo].[Datos] d 
                RIGHT JOIN (
                    SELECT 
                        s.[nombre] as [Sensor]
		                ,[Latitud]
		                ,[Longitud]
		                ,[FK_BaseStationid]
                        ,max(d.[stamp]) as [Ultimo] 
                    FROM [plataforma_iot].[dbo].[Datos] d 
                    RIGHT JOIN [plataforma_iot].[dbo].[Sensor] s ON d.[FK_Sensorid] = s.[id] 
                    JOIN [plataforma_iot].[dbo].[EstacionBase] eb ON s.[FK_BaseStationid] = eb.[id]
	                WHERE eb.[nombre] = 'EB01'
	                GROUP BY s.[nombre],[Latitud],[Longitud],[FK_BaseStationid]
                ) as ls ON d.[Stamp] = ls.[Ultimo]
                JOIN [plataforma_iot].[dbo].[EstacionBase] eb ON ls.[FK_BaseStationid] = eb.[id] 

                OPCION 2 (OPTIMA)

                SELECT 
	                x.[Sensor]
	                ,x.[Latitud]
	                ,x.[Longitud]
	                ,x.[stamp]
	                ,x.[Humedad]
	                ,x.[Temperatura]
                FROM
                (
	                SELECT 
		                s.[nombre] as [Sensor] ,[Latitud] ,[Longitud] ,d.[stamp], d.[humity] [Humedad], d.[temperature] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[nombre] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataforma_iot].[dbo].[Datos] d 
	                RIGHT JOIN [plataforma_iot].[dbo].[Sensor] s ON d.[FK_Sensorid] = s.[id] 
	                JOIN [plataforma_iot].[dbo].[EstacionBase] eb ON s.[FK_BaseStationid] = eb.[id]
	                WHERE eb.[nombre] = 'EB01'
                ) AS x
                WHERE x.[rk] = 1
                ORDER BY x.[Sensor]
            **/

            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@id", idEstacionBase}
            };
            string query = @"
                SELECT 
	                x.[NombreSensor]
	                ,x.[Latitud]
	                ,x.[Longitud]
	                ,x.[Fecha]
	                ,x.[Humedad]
	                ,x.[Temperatura]
                    ,x.[fk_idestacionbase]
                    ,x.[IdSensor]
                FROM
                (
	                SELECT 
		                RTRIM(s.[nombre]) as [NombreSensor], s.[fk_idestacionbase], s.[id] [IdSensor], [latitud] [Latitud],[longitud] [Longitud],d.[stamp] [Fecha], d.[humedad] [Humedad], d.[temperatura] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[nombre] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataforma_iot].[dbo].[Datos] d 
	                RIGHT JOIN [plataforma_iot].[dbo].[Sensor] s ON d.[fk_idsensor] = s.[id] 
	                WHERE s.[fk_idestacionbase] = @id
                ) AS x
                WHERE x.[rk] = 1
                ORDER BY x.[NombreSensor]
            ";

            IEnumerable<EntidadSensorResultado> resultado = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    resultado = await conn.QueryAsync<EntidadSensorResultado>(query, parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (ObtenerSensores) - {ex.Message}");
            }
            return resultado;
        }

        public async Task<bool> Eliminar(int idEstacionBase)
        {
            bool eliminada = false;

            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                { "@id", idEstacionBase}                
            };

            string query = string.Format(@"DELETE FROM [plataforma_iot].[dbo].[EstacionBase]
                                           WHERE [id] = @id");

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                    eliminada = true;
                    log.Information($"REPOSITORIO ESTACION BASE (Eliminar) - Se ha eliminado la estación base {idEstacionBase}");
                }                
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (Eliminar) - {ex.Message}");
                eliminada = false;
            }

            return eliminada;
        }

        public async Task<bool> Editar(EntidadEstacionBase estacionBase)
        {
            bool editado;
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", estacionBase.Id },
                { "@nombre", estacionBase.Nombre }
            };

            string query = @"UPDATE [plataforma_iot].[dbo].[EstacionBase]
                             SET [nombre] = @nombre
                             WHERE [id] = @id";

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
                log.Error($"ERR. REPOSITORIO ESTACIÓN BASE (Editar) - {ex.Message}");
                editado = false;
            }
            return editado;
        }

        public async Task Crear(EntidadEstacionBase estacionBase) //suponemos que el atributo fk_idproyecto es != null
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@nombre", estacionBase.Nombre },
                { "@fk_idproyecto", estacionBase.FK_IdProyecto }
            };

            string query = @" INSERT INTO [plataforma_iot].[dbo].[EstacionBase] ([nombre],[fk_idproyecto])
                              VALUES (@nombre, @fk_idproyecto)";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (Crear) - {ex.Message}");
            }           
        }

        public async Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string idUsuario)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                { "@id", idUsuario }
            };

            string query = @"   
                SELECT eb.[id], eb.[nombre], eb.[fk_idproyecto] 
                FROM [dbo].[EstacionBase] eb 
                INNER JOIN [dbo].[Usuario_en_Proyecto] uep ON eb.[fk_idproyecto] = uep.[id_proyecto]
                WHERE uep.[id_usuario] = @id";

            IEnumerable<EntidadEstacionBase> estaciones = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    estaciones = await conn.QueryAsync<EntidadEstacionBase>(query, parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO ESTACION BASE (ObtenerEstacionesBase) - {ex.Message}");
            }
            return estaciones;
        }
    }
}