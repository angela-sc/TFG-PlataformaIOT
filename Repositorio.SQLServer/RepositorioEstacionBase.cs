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

namespace Repositorio.SQLServer
{
    public class RepositorioEstacionBase : IRepositorioEstacionBase
    {
        private string connectionString;
        private ILogger log;

        public RepositorioEstacionBase(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.log = logger;
        }
        public async Task<int> GetId(string nombreEstacionBase)
        {
            string query = String.Format( "SELECT [Id] FROM [plataformadb].[dbo].[Base_station] WHERE [Name]= '{0}'", nombreEstacionBase);
           
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var res = await conn.QueryAsync<int>(query);
                    return res.FirstOrDefault();
                }
            }
            catch(Exception)
            {
                log.Warning($"No se ha encontrado ningun id para la estacion base {nombreEstacionBase}");
                return -1;
            }        
        }

        public void InsertaEstacion(EntidadEstacionBase entidadEstacion)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase)
        {
            /*SELECT 
	                s.[Name]			as [NombreSensor]
	                ,s.[Location]		as [Coordenada]
	                ,d.[Stamp]			as [Fecha]
	                ,d.[humity]			as [Humedad]
	                ,d.[temperature]	as [Temperatura]
            FROM [plataformadb]..[Data] d
            JOIN [plataformadb]..[Sensor] s ON d.[FK_SensorId] = s.[Id]
            JOIN [plataformadb]..[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id]
            JOIN (
	            SELECT 
		            s.[Name] as [NombreSensor]
		            ,max(d.[stamp]) as [Ultimo]
	            FROM [plataformadb]..[Data] d
	            JOIN [plataformadb]..[Sensor] s ON d.[FK_SensorId] = s.[Id]
	            JOIN [plataformadb]..[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id]
	            WHERE eb.[Name] = 'EB01'
	            GROUP BY s.[Name]
            ) as ls ON d.[Stamp] = ls.[Ultimo] AND s.[Name] = ls.[NombreSensor] 
            */


            string query = String.Format("SELECT s.[Name] as [NombreSensor], s.[Latitud], s.[Longitud], d.[Stamp] as [Fecha], d.[humity] as [Humedad], d.[temperature] as [Temperatura] FROM [plataformadb].[dbo].[Data] d JOIN [plataformadb].[dbo].[Sensor] s ON d.[FK_SensorId] = s.[Id] JOIN [plataformadb].[dbo].[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id] JOIN (SELECT s.[Name] as [NombreSensor], max(d.[stamp]) as [Ultimo] FROM [plataformadb].[dbo].[Data] d JOIN [plataformadb].[dbo].[Sensor] s ON d.[FK_SensorId] = s.[Id] JOIN [plataformadb].[dbo].[Base_Station] eb ON s.[FK_BaseStationId] = eb.[Id] WHERE eb.[Name] = '{0}' GROUP BY s.[Name] ) as ls ON d.[Stamp] = ls.[Ultimo] AND s.[Name] = ls.[NombreSensor]", nombreEstacionBase);

            IEnumerable<EntidadSensorResultado> resultado = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
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

        //public async Task<IEnumerable<EntidadCoordenada>> ObtenerCoordenadasSensores (string nombreEstacionBase)
        //{
        //    /*  SELECT s.[Name], s.[Latitud], s.[Longitud] from [plataformadb]..[Sensor] s
        //        JOIN [plataformadb]..[Base_Station] eb ON eb.Id = s.FK_BaseStationId WHERE eb.[Name] = 'EB01'
        //        ORDER BY s.[Name]
        //    */

        //    string query = String.Format("SELECT s.[Name], s.[Latitud], s.[Longitud] from [plataformadb].[dbo].[Sensor] s JOIN [plataformadb].[dbo].[Base_Station] eb ON eb.[Id] = s.[FK_BaseStationId] WHERE eb.[Name] = '{0}' ORDER BY s.[Name]", nombreEstacionBase);

        //    IEnumerable<EntidadCoordenada> resultado = null;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            resultado = await conn.QueryAsync<EntidadCoordenada>(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return resultado;
        //}
    }
}