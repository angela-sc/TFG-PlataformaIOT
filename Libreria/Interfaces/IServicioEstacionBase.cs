using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
   public interface IServicioEstacionBase
    {
        Task<string> Nombre(int idEstacionBase);
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        Task<IEnumerable<EntidadEstacionBase>> ListaEstacionesBase(string nombreProyecto);
        Task<bool> Eliminar(int idEstacionBase);
        Task<bool> Editar(EntidadEstacionBase estacionBase);
        Task Crear(EntidadEstacionBase estacionBase);
    }
}
