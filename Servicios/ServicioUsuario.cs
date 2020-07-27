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

        //Llama a RepositorioUsuario con los datos del usuario para registrarlo
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
    }
}
