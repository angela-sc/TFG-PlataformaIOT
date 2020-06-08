using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioSensor
    {
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, int top);
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor, DateTime? fechaInicio, DateTime? fechaFin);
        Task<IEnumerable<double>> ObtenerTemperatura(int idSensor, int top);
        Task<int> ObtenerId(string nombreSensor, int idEstacionBase);
        Task<string> ObtenerNombre(int idSensor, int idEstacionBase);
        Task<bool> EliminarDatos(int fk_SensorId);
        Task<bool> EliminarSensor(int sensorid);
        Task<bool> Editar(EntidadSensor sensor);
        Task Crear(EntidadSensor sensor);
    }
}