﻿using Libreria.Entidades;
using Libreria.Interfaces;
using Repositorio.SQLServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioEstacionBase : IServicioEstacionBase
    {
        private IRepositorioEstacionBase repositorioEstacionBase;

        private string cadenaConexion;
        private ILogger log;

        public ServicioEstacionBase(string cadenaConexion, ILogger log)
        {
            this.cadenaConexion = cadenaConexion;
            this.log = log;
            repositorioEstacionBase = new RepositorioEstacionBase(cadenaConexion, log);
        }

        public async Task<string> Nombre (int idEstacionBase)
        {
            string nombre = await repositorioEstacionBase.ObtenerNombre(idEstacionBase);
            return nombre;
        }

        public async Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(int idEstacionBase)
        {
            IEnumerable<EntidadSensorResultado> sensores = new List<EntidadSensorResultado>();
            sensores = await repositorioEstacionBase.ObtenerSensores(idEstacionBase);

            return sensores;
        }
        
        public async Task<IEnumerable<EntidadEstacionBase>> ListaEstacionesBase(int idProyecto) //Metodo para obtener la lista de estaciones base pertenecientes a un proyecto > ¿pasamos id o nombre del proyeecto?
        {
            IEnumerable<EntidadEstacionBase> estacionesBase = new List<EntidadEstacionBase>();
            try
            {
                estacionesBase = await repositorioEstacionBase.ObtenerEstacionesBase(idProyecto);
            }
            catch (Exception)
            {
                estacionesBase = null;
            }

            return estacionesBase;
        }

        public async Task<bool> Eliminar(int idEstacionBase)
        {
            bool eliminada;
            try
            {
                eliminada = await repositorioEstacionBase.Eliminar(idEstacionBase);
            }
            catch (Exception)
            {
                eliminada = false;
            }
            return eliminada;
        }

        public async Task<bool> Editar(EntidadEstacionBase estacionBase)
        {
            return await repositorioEstacionBase.Editar(estacionBase);
        }

        public async Task Crear(EntidadEstacionBase estacionBase)
        {
            await repositorioEstacionBase.Crear(estacionBase);            
        }
    }
}
