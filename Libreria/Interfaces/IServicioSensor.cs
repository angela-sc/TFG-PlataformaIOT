using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioSensor
    {
        Task<List<EntidadDatoBase>> ObtenerDatos(int idSensor);
        //Task<List<EntidadDatoBase>> BuscarDatos(int idSensor, DateTime inicio, DateTime fin);
    }
}
