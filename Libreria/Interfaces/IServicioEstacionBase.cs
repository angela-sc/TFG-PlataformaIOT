using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
   public interface IServicioEstacionBase
    {
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        Task<IEnumerable<EntidadEstacionBase>> ListaEstacionesBase(string nombreProyecto);
        Task<bool> EliminarEstacionBase(int idEstacionBase);
        Task<bool> Editar(EntidadEstacionBase estacionBase);
    }
}
