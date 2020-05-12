﻿using Libreria.Entidades;
using Libreria.Interfaces;
using Newtonsoft.Json;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioProyecto : IServicioProyecto
    {
        private ILogger log;
        private IRepositorioProyecto repositorioProyecto;

        private IRepositorioSensor repositorioSensor;

        public ServicioProyecto(string cadenaConexion, ILogger logger)
        {
            this.log = logger;
            this.repositorioProyecto = new RepositorioProyecto(cadenaConexion, log);

            this.repositorioSensor = new RepositorioSensor(cadenaConexion, log);
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

        public async Task CrearProyecto(EntidadProyecto proyecto)
        {
           
            //bool insertado = false;
            try
            {
                 await repositorioProyecto.InsertaProyecto(proyecto);
                //insertado = true;
            }
            catch (Exception)
            {
                //log.Warning($"Problema al obtener los proyectos del usuario {idUsuario}. No tiene ningún proyecto asociado.");
                
            }
            //return insertado;
        }

        public async Task<bool> EliminarProyecto(int idProyecto)
        {
            bool eliminado = false;

            try
            {
                eliminado = await repositorioProyecto.EliminarProyecto(idProyecto);
                return eliminado;
            }
            catch (Exception)
            {

                return eliminado = false;
            }
        }

        public async Task<bool> EditarProyecto(EntidadProyecto proyecto)
        {
            return await repositorioProyecto.EditarProyecto(proyecto);      
        }
        
    }
}
