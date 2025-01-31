﻿using Dapper;
using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.SQLServer
{
    public class RepositorioProyecto : IRepositorioProyecto
    {
        private string cadenaConexion;
        private ILogger log;

        public RepositorioProyecto(string cadenaConexion, ILogger logger)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = logger;
        }
       
        public async Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(string idUsuario)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idUsuario }              
            };

            string query = @"SELECT p.[nombre] [Nombre], p.[descripcion] [Descripcion], p.[id] [Id]
                                FROM [plataforma_iot].[dbo].[Usuario_en_Proyecto] uip
                                JOIN [plataforma_iot].[dbo].[Proyecto] p ON uip.[id_proyecto] = p.[id]
                                WHERE uip.[id_usuario] = @id";

            IEnumerable<EntidadProyecto> resultado = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    resultado = await conn.QueryAsync<EntidadProyecto>(query,parametros);
                }
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO PROYECTO (ObtenerProyectos) - {ex.Message}");
            }
            return resultado;
        }

        public async Task<bool> EliminarProyecto(int idProyecto)
        {
            bool eliminado = false;

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", idProyecto }
            };

            string query = string.Format(@"DELETE FROM [plataforma_iot].[dbo].[Proyecto]
                                           WHERE [id] = @id");
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    await conn.ExecuteAsync(query,parametros);
                    eliminado = true;
                }               
            }
            catch (Exception ex)
            {
                log.Error($"ERR. REPOSITORIO PROYECTO (EliminarProyecto) - {ex.Message}");
                eliminado = false;
            }
            return eliminado;
        }

        public async Task<bool> EditarProyecto(EntidadProyecto proyecto)
        {
            bool editado;
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@id", proyecto.Id },
                { "@nombre", proyecto.Nombre },
                { "@descripcion", proyecto.Descripcion}
            };

            string query = @"   UPDATE [plataforma_iot].[dbo].[Proyecto]
                                SET [nombre] = @nombre, [descripcion] = @descripcion
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
                log.Error($"ERR. REPOSITORIO PROYECTO (EditarProyecto) - {ex.Message}");
                editado = false;
            }
            return editado;
        }

        public async Task<bool> InsertaProyecto(EntidadProyecto proyecto, string idUsuario)
        {
            bool insertado;
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@nombre", proyecto.Nombre },
                { "@descripcion", proyecto.Descripcion},
                { "@idusuario", idUsuario}
            };

            string query = @"BEGIN TRAN
                             DECLARE @idproyecto int = -1
                             INSERT INTO [plataforma_iot].[dbo].[Proyecto] ([nombre],[descripcion]) VALUES (@nombre, @descripcion)
                             SELECT @idproyecto = SCOPE_IDENTITY()
                             INSERT INTO [plataforma_iot].[dbo].[Usuario_en_Proyecto] ([id_usuario],[id_proyecto]) VALUES (@idusuario, @idproyecto)
                             COMMIT TRAN";
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
                log.Error($"ERR. REPOSITORIO PROYECTO (InsertaProyecto) - {ex.Message}");
                insertado = false;
            }
            return insertado;
        }       
    }
}
