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
        //public async Task InsertaProyecto(EntidadProyecto proyecto)
        //{                
        //    Dictionary<string, object> parametros = new Dictionary<string, object>
        //    {
        //        { "@nombre", proyecto.Nombre },
        //        { "@descripcion", proyecto.Descripcion}
                
        //    };

        //    string query = @"INSERT INTO [plataforma_iot].[dbo].[Proyecto] ([nombre],[descripcion])
        //                    VALUES (@nombre, @descripcion)";
        //    try
        //    {
        //        using(SqlConnection conn = new SqlConnection(cadenaConexion))
        //        {
        //            await conn.ExecuteAsync(query, parametros);
        //        }             
        //    }
        //    catch(Exception ex)
        //    {
        //        //log.Error($"Se ha producido un error al insertar el proyecto {proyecto.name} en la base de datos - ERR. REPOSITORIO PROYECTO");
        //        Console.WriteLine("Error en el método InsertaProyecto "+ex.Message); // --ELIMINAR CUANDO SE PASE EL LOG   
        //    }
        //}

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

        //public async Task AsociarUsuarioProyecto(int idProyecto, int idUsuario)
        //{
        //    Dictionary<string, object> parametros = new Dictionary<string, object>
        //    {
        //        { "@idproyecto", idProyecto },
        //        { "@idusuario", idUsuario}

        //    };

        //    string query = @"INSERT INTO [plataforma_iot].[dbo].[Usuario_en_Proyecto] ([id_usuario],[id_proyecto])
        //                    VALUES (@idusuario, @idproyecto)";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(cadenaConexion))
        //        {
        //            await conn.ExecuteAsync(query, parametros);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.Error($"Se ha producido un error al insertar el proyecto {proyecto.name} en la base de datos - ERR. REPOSITORIO PROYECTO");
        //        Console.WriteLine("Error en el método AsociarUsuarioProyecto " + ex.Message); // --ELIMINAR CUANDO SE PASE EL LOG   
        //    }

        //}

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
                //Console.WriteLine("Error en el método InsertaProyecto " + ex.Message); // --ELIMINAR CUANDO SE PASE EL LOG   
            }
            return insertado;
        }

        //public async Task<int> ObtenerId(string Proyecto)
        //{
        //    Dictionary<string, object> parametros = new Dictionary<string, object>
        //    {
        //        { "@nombre", Proyecto }
        //    };

        //    string query = @"SELECT [id] FROM [plataforma_iot].[dbo].[Proyecto] WHERE [nombre]=@nombre";

        //    //IEnumerable<EntidadProyecto> resultado = null;
        //    int resultado = -1;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(cadenaConexion))
        //        {
        //            var r = await conn.QueryAsync<int>(query, parametros);
        //            resultado = r.FirstOrDefault();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.Error($"Error al obtener los proyectos del usuario {idUsuario} - ERR. REPOSITORIO PROYECTO");
        //        Console.WriteLine(ex.Message, "Error en obtener id de Respositorio proyecto: ");
        //        resultado = -1;
        //    }
        //    return resultado;
        //}
    }
}
