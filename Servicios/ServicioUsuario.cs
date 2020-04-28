using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private ILogger log;
        private IRepositorioUsuario repositorioUsuario;

        public ServicioUsuario(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioUsuario = new RepositorioUsuario(cadenaConexion, log);
        }

        /*
         * Metodo que llama a RepositorioUsuario con los datos del usuario para registrarlo
         * Devuelvo true si ha ido todo ok, false en otro caso.
         */
        public async Task<bool> RegistraUsuario(EntidadUsuario usuario)
        {
            bool registrado = false;

            try
            {
                registrado = await repositorioUsuario.InsertaUsuario(usuario);
            }
            catch (Exception)
            {
                //TODO
            }

            return registrado;
        }

        ////Metodo que obtienen los proyectos de un usuario
        //public async Task<IEnumerable<EntidadProyecto>> Proyectos(int idUsuario)
        //{
        //    IEnumerable<EntidadProyecto> proyectos = null;
        //    try
        //    {
        //        proyectos = await repositorioUsuario.ObtenerProyectosAsync(idUsuario);

        //    }
        //    catch (Exception)
        //    {
        //        log.Error($"Error en el servicio de usuario: no se han podido obtener los proyectos del usuario {idUsuario}");
        //    }
        //    return proyectos;
        //}
    }
}
