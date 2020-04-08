using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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
        public void InsertaProyecto(EntidadProyecto proyecto)
        {

            Dictionary<string, object> queryParams = new Dictionary<string, object>
            {
                { "@name", proyecto.name },
                { "@description", proyecto.description}
            };

            string query = @"INSERT INTO [Project] ([Name],[Description])
                            VALUES (@name, @description)";
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.ExecuteAsync(query, queryParams);
                }
            }catch(Exception ex)
            {
                //Console.WriteLine("Error en el método InsertaProyecto "+ex.Message);
                log.Error($"Se ha producido un error al insertar el proyecto {proyecto.name} en la base de datos.");
            }

            
        }
    }
}
