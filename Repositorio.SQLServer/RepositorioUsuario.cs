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
                { "@nombre", usuario.Nombre },
                { "@apellidos", usuario.Apellidos },
                { "@email", usuario.Email},
                { "@contrasenya", usuario.Contrasenya }       
            };

            string query = @"INSERT INTO [plataforma_iot].[dbo].[Usuario] ([email],[contrasenya],[nombre],[apellidos]) VALUES (@email, @contrasenya,@nombre,@apellidos)";

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query, parametros);
                    insertado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO USUARIO (InsertaUsuario) - {ex.Message}");
                return insertado; //¿esto se puede quitar?
            }
            return insertado;
        }

    }
}
