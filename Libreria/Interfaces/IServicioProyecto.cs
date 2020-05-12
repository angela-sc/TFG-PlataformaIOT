using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioProyecto
    {
        Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(int idUsuario); //metodo para obtener los proyectos propios de un usuario
        Task CrearProyecto(EntidadProyecto proyecto);

        Task<bool> EditarProyecto(EntidadProyecto proyecto);
    }
}
