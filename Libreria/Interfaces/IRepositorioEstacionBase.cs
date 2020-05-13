using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioEstacionBase
    {
        Task InsertaEstacionBase(EntidadEstacionBase estacionBase); 
        Task<int> ObtenerId(string estacionBase);
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto);

        Task<bool> Eliminar(int idEstacionBase);
        Task<bool> Editar(EntidadEstacionBase estacionBase);

    }
}
