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
        public async Task<int> ObtenerId(string estacionBase)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@nombre", estacionBase}
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
                log.Warning($"No se ha encontrado ningun id para la estacion base {estacionBase}");
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
                FROM [plataformadb].[dbo].[Data] d 
                RIGHT JOIN (
                    SELECT 
                        s.[Name] as [NombreSensor]
		                ,[Latitud]
		                ,[Longitud]
		                ,[FK_BaseStationId]
                        ,max(d.[stamp]) as [Ultimo] 
                    FROM [plataformadb].[dbo].[Data] d 
                    RIGHT JOIN [plataformadb].[dbo].[Sensor] s ON d.[FK_SensorId] = s.[Id] 
                    JOIN [plataformadb].[dbo].[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id]
	                WHERE eb.[Name] = 'EB01'
	                GROUP BY s.[Name],[Latitud],[Longitud],[FK_BaseStationId]
                ) as ls ON d.[Stamp] = ls.[Ultimo]
                JOIN [plataformadb].[dbo].[Base_Station] eb ON ls.[FK_BaseStationId] = eb.[Id] 

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
		                s.[Name] as [NombreSensor] ,[Latitud] ,[Longitud] ,d.[stamp], d.[humity] [Humedad], d.[temperature] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[Name] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataformadb].[dbo].[Data] d 
	                RIGHT JOIN [plataformadb].[dbo].[Sensor] s ON d.[FK_SensorId] = s.[Id] 
	                JOIN [plataformadb].[dbo].[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id]
	                WHERE eb.[Name] = 'EB01'
                ) AS x
                WHERE x.[rk] = 1
                ORDER BY x.[NombreSensor]
            **/

            string query = String.Format(@"
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
		                RTRIM(s.[Name]) as [NombreSensor] ,[Latitud] ,[Longitud] ,d.[stamp] [Fecha], d.[humity] [Humedad], d.[temperature] [Temperatura]
		                ,DENSE_RANK() OVER(PARTITION BY s.[Name] ORDER BY d.[stamp] DESC) AS [rk]
	                FROM [plataformadb].[dbo].[Data] d 
	                RIGHT JOIN [plataformadb].[dbo].[Sensor] s ON d.[FK_SensorId] = s.[Id] 
	                JOIN [plataformadb].[dbo].[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id]
	                WHERE eb.[Name] = '{0}'
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
            }
            return resultado;
        }

        public async Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>() 
            {
                {"@proyecto", nombreProyecto}
            };
            string query = @"
                             SELECT eb.* FROM [plataformadb].[dbo].[Base_Station] eb
                             JOIN [plataformadb].[dbo].[Project] p ON eb.[FK_ProjectId] = p.[Id]
                             WHERE p.[Name] = @proyecto
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
                //log.Error($"Error en el repositorio de estación base: no se ha podido obtener la lista del sensor {nombreProyecto}");
            }
            return estaciones;
        }
    }
}