﻿using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioEstacionBase
    {
        void InsertaEstacion(EntidadEstacionBase entidadEstacion);
        //Task<int> GetId(string nombreEstacionBase);
        Task<int> ObtenerId(string estacionBase);
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        // Task<IEnumerable<EntidadCoordenada>> ObtenerCoordenadasSensores(string nombreEstacionBase);

        Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto);

    }
}
