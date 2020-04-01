using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repositorio.SQLServer
{
    public class RepositorioProyecto : IRepositorioProyecto
    {
        private string connectionString;

        public RepositorioProyecto(string connectionString)
        {
            this.connectionString = connectionString;
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
                Console.WriteLine("Error en el método InsertaProyecto "+ex.Message);
            }

            
        }
    }
}
