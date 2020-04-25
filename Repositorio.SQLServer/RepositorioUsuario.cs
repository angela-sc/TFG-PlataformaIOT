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
        public void InsertaUsuario(EntidadUsuario usuario)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@name", usuario.name },
            };

            try
            {


            }
            catch (Exception)
            {
                log.Error($"Se ha producido un erro al insertar el usuario {usuario.email} en la base de datos.");
            }
            //throw new NotImplementedException();
        }

        ////Método que devuelve la lista de proyectos del usuario (tabla usuario_en_proyecto)
        //public async Task<IEnumerable<EntidadProyecto>> ObtenerProyectosAsync(int idUsuario)
        //{
        //    IEnumerable<EntidadProyecto> proyectos = null;

        //    Dictionary<string, object> parametros = new Dictionary<string, object>
        //    {
        //        {"@id",  idUsuario}
        //    };

        //    string query = String.Format(@"SELECT p.[Name],p.[Description]
        //                                   FROM [plataformadb].[dbo].[Project] p
        //                                    JOIN [plataformadb].[dbo].[User_In_Project] uip ON uip.[ProjectId] = p.[Id] 
        //                                    WHERE uip.UserId = @id
        //                                   GROUP BY p.[Name],p.[Description]");

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(cadenaConexion))
        //        {
        //            proyectos = await conn.QueryAsync<EntidadProyecto>(query, parametros);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //log.Error($"Se ha producido un erro al recuperar los proyectos del usuario {idUsuario} de la base de datos.");
        //    }
        //    return proyectos;
        //}

    }
}
