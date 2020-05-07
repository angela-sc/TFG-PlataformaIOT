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

        //Metodo que obtiene el id de la estación base a partir de su nombre
        public async Task<int> ObtenerId(string nombreEstacionBase)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@nombre", nombreEstacionBase}
            };

            string query = string.Format(@"SELECT [id] FROM [plataforma_iot].[dbo].[EstacionBase]
                                            WHERE [nombre] = @nombre");

            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    var resultado = await con.QueryAsync<int>(query);
                    return resultado.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                log.Warning($"No se ha encontrado ningún id para la estacion base {nombreEstacionBase} - ERR. REPOSITORIO ESTACION BASE");
                return -1;
            }
        }

        public void InsertaEstacion(EntidadEstacionBase entidadEstacion)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase)
        {
            /**
                OPCION 1

                SELECT  
	                ls.[NombreSensor] 
                    ,ls.[Latitud]
                    ,ls.[Longitud]
                    ,d.[Stamp] as [Fecha]
                    ,d.[humity] as [Humedad]
                    ,d.[temperature] as [Temperatura] 
                FROM [plataforma_iot].[dbo].[Datos] d 
                RIGHT JOIN (
                    SELECT 
                        s.[nombre] as [NombreSensor]
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
	                x.[NombreSensor]
	                ,x.[Latitud]
	                ,x.[Longitud]
	                ,x.[stamp]
	                ,x.[Humedad]
	                ,x.[Temperatura]
                FROM
                (
	                SELECT 
		                s.[nombre] as [NombreSensor] ,[Latitud] ,[Longitud] ,d.[stamp], d.[humity] [Humedad], d.[temperature] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[nombre] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataforma_iot].[dbo].[Datos] d 
	                RIGHT JOIN [plataforma_iot].[dbo].[Sensor] s ON d.[FK_Sensorid] = s.[id] 
	                JOIN [plataforma_iot].[dbo].[EstacionBase] eb ON s.[FK_BaseStationid] = eb.[id]
	                WHERE eb.[nombre] = 'EB01'
                ) AS x
                WHERE x.[rk] = 1
                ORDER BY x.[NombreSensor]
            **/

            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@nombre", nombreEstacionBase}
            };
            string query = string.Format(@"
                SELECT 
	                x.[NombreSensor]
	                ,x.[Latitud]
	                ,x.[Longitud]
	                ,x.[Fecha]
	                ,x.[Humedad]
	                ,x.[Temperatura]
                FROM
                (
	                SELECT 
		                RTRIM(s.[nombre]) as [NombreSensor] ,[latitud] [Latitud],[longitud] [Longitud],d.[stamp] [Fecha], d.[humedad] [Humedad], d.[temperatura] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[nombre] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataforma_iot].[dbo].[Datos] d 
	                RIGHT JOIN [plataforma_iot].[dbo].[Sensor] s ON d.[fk_idsensor] = s.[id] 
	                JOIN [plataforma_iot].[dbo].[EstacionBase] eb ON s.[fk_idestacionbase] = eb.[id]
	                WHERE eb.[nombre] = @nombre
                ) AS x
                WHERE x.[rk] = 1
                ORDER BY x.[NombreSensor]
            ", nombreEstacionBase);

            IEnumerable<EntidadSensorResultado> resultado = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    resultado = await conn.QueryAsync<EntidadSensorResultado>(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log.Error($"No se ha podido obtener la lista de sensores de la estación base '{nombreEstacionBase}' - ERR. REPOSITORIO ESTACION BASE");
            }
            return resultado;
        }

        //A partir del nombre del proyecto obtiene la estacion base
        public async Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>() 
            {
                {"@proyecto", nombreProyecto} 
            };
            string query = @"
                             SELECT eb.* FROM [plataforma_iot].[dbo].[EstacionBase] eb
                             JOIN [plataforma_iot].[dbo].[Proyecto] p ON eb.[fk_idproyecto] = p.[id]
                             WHERE p.[nombre] = @proyecto
                            ";

            IEnumerable<EntidadEstacionBase> estaciones = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    estaciones = await conn.QueryAsync<EntidadEstacionBase>(query, parametros);
                }
            }
            catch (Exception)
            {
                //log.Error($"No se ha podido obtener la lista de estaciones base del proyecto '{nombreProyecto}' - ERR. REPOSITORIO ESTACION BASE");
            }
            return estaciones;
        }
    }
}