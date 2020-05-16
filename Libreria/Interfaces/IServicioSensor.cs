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
        //Task<List<EntidadDatoBase>> BuscarDatos(int idSensor, DateTime inicio, DateTime fin);
        Task<IEnumerable<double>> ObtenerTemperatura(int idSensor, int top);
        Task<int> ObtenerIdSensor(string nombreSensor, string nombreEstacionBase);
        Task<bool> EliminarDatos(int fk_SensorId);
        Task<bool> EliminarSensor(int sensorid);
        Task<bool> Editar(EntidadSensor sensor);
        Task Crear(EntidadSensor sensor);
    }
}