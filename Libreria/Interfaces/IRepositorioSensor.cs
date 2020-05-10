using Libreria.Entidades;
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
        Task<int> ObtenerId(string nombreSensor, int idEstacionBase);
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top);

        //Metodos para eliminar un sensor y sus datos
        Task<bool> EliminarDatos(int fk_sensorid);
        Task<bool> EliminarSensor(int sensorid);

    }
}
