﻿using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioProyecto
    {
        private ILogger log;
        private IRepositorioProyecto repositorioProyecto;

        public ServicioProyecto(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioProyecto = new RepositorioProyecto(cadenaConexion, log);
        }

        public async Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(int idUsuario)
        {
            IEnumerable<EntidadProyecto> proyectos = null;

            try
            {
                proyectos = await repositorioProyecto.ObtenerProyectos(idUsuario);
            }
            catch (Exception)
            {
                log.Warning($"Problema al obtener los proyectos del usuario {idUsuario}. No tiene ningún proyecto asociado.");
            }

            return proyectos; //devuelve null o un IEnumerable<EntidadDatoBase>
        }

        public  void CrearProyecto(EntidadProyecto proyecto)
        {
           
            //bool insertado = false;
            try
            {
                 repositorioProyecto.InsertaProyecto(proyecto);
                //insertado = true;
            }
            catch (Exception)
            {
                //log.Warning($"Problema al obtener los proyectos del usuario {idUsuario}. No tiene ningún proyecto asociado.");
                
            }
            //return insertado;
        }
    }
}
