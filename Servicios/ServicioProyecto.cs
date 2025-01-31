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

        public async Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(string idUsuario)
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

        public async Task<bool> Crear(EntidadProyecto proyecto, string idUsuario)
        {
            bool creado;
            try
            {
                creado = await repositorioProyecto.InsertaProyecto(proyecto, idUsuario);
            }
            catch (Exception ex)
            {
                //log.Warning($"Problema al obtener los proyectos del usuario {idUsuario}. No tiene ningún proyecto asociado.");
                creado = false;
                log.Error($"ERR. SERVICIO PROYECTO (Crear) - {ex.Message}");
            }

            return creado;
        }

        public async Task<bool> Eliminar(int idProyecto)
        {
            bool eliminado = false;

            try
            {
                eliminado = await repositorioProyecto.EliminarProyecto(idProyecto);
                return eliminado;
            }
            catch (Exception ex)
            {
                log.Error($"ERR. SERVICIO PROYECTO (Eliminar) - {ex.Message}");
                return eliminado = false;
            }
        }

        public async Task<bool> Editar(EntidadProyecto proyecto)
        {
            bool editado;
            try
            {
                editado = await repositorioProyecto.EditarProyecto(proyecto);
            }
            catch(Exception ex)
            {
                editado = false;
                log.Error($"ERR. SERVICIO PROYECTO (Editar) - {ex.Message}");
            }
            return editado; 
        }
        
    }
}
