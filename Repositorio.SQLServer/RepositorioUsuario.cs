using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.SQLServer
{
    public class RepositorioUsuario : IRepositorioUsuario
    {

        private string cadenaConexion;
        private ILogger log;

        public RepositorioUsuario(string cadenaConexion, ILogger logger)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = logger;
        }

        
        /*
         * Metodo que inserta un usuario en la base de datos.
         * Devuelve true si no se produce ningún error, false en cualquier otro caso.
         */
        public async Task<bool> InsertaUsuario(EntidadUsuario usuario)
        {
            bool insertado = false;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@Name", usuario.name },
                { "@Surname", usuario.surname },
                { "@Email", usuario.email},
                { "@Password", usuario.password }       
            };

            string query = string.Format(@"INSERT INTO [plataformadb].[dbo].[User] ([Email],[Password],[Name],[Surname]) VALUES (@Email, @Password,@Name,@Surname)");

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                    insertado = true;
                }
            }
            catch (Exception)
            {
                log.Error($"Se ha producido un erro al insertar el usuario {usuario.email} en la base de datos.");
                return insertado; //¿esto se puede quitar?
            }
            return insertado;
        }

    }
}
