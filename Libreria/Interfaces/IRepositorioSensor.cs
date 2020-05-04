﻿using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioSensor
    {
        Task<bool> InsertaSensor(EntidadSensor sensor);
        Task<bool> InsertaDato(EntidadDato dato);
        Task<int> GetId(string nombreSensor, int idEstacionBase);
        Task<IEnumerable<EntidadDatoBase>> GetData(int idSensor, int top);

        Task<bool> EliminarDatos(int fk_sensorid);

    }
}
