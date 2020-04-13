using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioSensor
    {
        Task<IEnumerable<EntidadDatoBase>> ObtenerDatos(int idSensor);
        //Task<List<EntidadDatoBase>> BuscarDatos(int idSensor, DateTime inicio, DateTime fin);
        Task<IEnumerable<double>> ObtenerTemperatura(int idSensor);
    }
}
