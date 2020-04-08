using Libreria.Entidades;
using Libreria.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
    }
}
