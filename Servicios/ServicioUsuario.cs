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
