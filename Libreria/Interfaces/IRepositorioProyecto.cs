using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioProyecto
    {        
        Task<bool> EditarProyecto(EntidadProyecto proyecto);             
        Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(string idUsuario); //Metodo para obtener todos los proyectos de un usuario determinado
        Task<bool> EliminarProyecto(int idProyecto); //Metodo para eliminar un proyecto - elimina las estaciones base y sensores asociados al proyecto
        Task<bool> InsertaProyecto(EntidadProyecto proyecto, string idUsuario);
    }
}
