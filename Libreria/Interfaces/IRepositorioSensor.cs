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
        Task<string> ObtenerNombre(int idSensor, int idEstacionBase);
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top);
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, DateTime? fechaInicio, DateTime? fechaFin); //filtra los datos segun fecha de inicio y fin

        //Metodos para eliminar un sensor y sus datos
        Task<bool> EliminarDatos(int fk_sensorid);
        Task<bool> EliminarSensor(int sensorid);
        Task<bool> Editar(EntidadSensor sensor);

    }
}
