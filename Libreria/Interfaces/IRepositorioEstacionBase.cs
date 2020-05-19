using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioEstacionBase
    {
        Task Crear(EntidadEstacionBase estacionBase); 
        Task<int> ObtenerId(string estacionBase);
        Task<string> ObtenerNombre(int idEstacionBase);
        Task<IEnumerable<EntidadSensorResultado>> ObtenerSensores(string nombreEstacionBase);
        Task<IEnumerable<EntidadEstacionBase>> ObtenerEstacionesBase(string nombreProyecto);

        Task<bool> Eliminar(int idEstacionBase);
        Task<bool> Editar(EntidadEstacionBase estacionBase);

    }
}
