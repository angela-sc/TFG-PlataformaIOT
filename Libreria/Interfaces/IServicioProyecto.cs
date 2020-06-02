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
        Task Crear(EntidadProyecto proyecto, int idUsuario);        
        Task<bool> EliminarProyecto(int idProyecto);
        Task<bool> EditarProyecto(EntidadProyecto proyecto);       
    }
}
