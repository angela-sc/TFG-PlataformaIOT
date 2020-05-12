using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioEstacionBase
    {
        void InsertaEstacion(EntidadEstacionBase entidadEstacion); // -- no está implementado
        Task<int> ObtenerId(string estacionBase);
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto);

        Task<bool> Eliminar(int idEstacionBase);
        Task<bool> Editar(EntidadEstacionBase estacionBase);

    }
}
