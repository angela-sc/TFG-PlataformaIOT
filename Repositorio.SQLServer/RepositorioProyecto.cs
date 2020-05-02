using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.SQLServer
{
    public class RepositorioProyecto : IRepositorioProyecto
    {
        private string connectionString;
        private ILogger log;

        public RepositorioProyecto(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.log = logger;
        }
        public async void InsertaProyecto(EntidadProyecto proyecto)
        {                
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@name", proyecto.name },
                { "@description", proyecto.description}
                
            };

            string query = @"INSERT INTO [plataformadb].[dbo].[Project] ([Name],[Description])
                            VALUES (@name, @description)";
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.ExecuteAsync(query, queryParams);
                }             
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error en el método InsertaProyecto "+ex.Message);
                //log.Error($"Se ha producido un error al insertar el proyecto {proyecto.name} en la base de datos.");              
            }
        }

        public async Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(int idUsuario)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@id", idUsuario }              
            };

            string query = String.Format(@"
                            SELECT p.[Name] [name], p.[Description] [description]
                            FROM [plataformadb].[dbo].[User_In_Project] uip
                            JOIN [plataformadb].[dbo].[Project] p ON uip.[ProjectId] = p.[Id]
                            WHERE uip.[UserId] = @id");

            IEnumerable<EntidadProyecto> resultado = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    resultado = await conn.QueryAsync<EntidadProyecto>(query,queryParams);
                }
            }
            catch (Exception ex)
            {
                //log.Error($"Se ha producido un error al obtener los proyectos.");
                Console.WriteLine(ex.Message, "Error: ");
            }
            return resultado;
        }
    }
}
