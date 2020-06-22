using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioProyecto
    {
        Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(string idUsuario); //metodo para obtener los proyectos propios de un usuario
        Task Crear(EntidadProyecto proyecto, string idUsuario);        
        Task<bool> Eliminar(int idProyecto);
        Task<bool> Editar(EntidadProyecto proyecto);       
    }
}
