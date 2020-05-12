using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioProyecto
    {
        Task InsertaProyecto(EntidadProyecto proyecto);
        
        Task<bool> EditarProyecto(EntidadProyecto proyecto);

        //Metodo para obtener todos los proyectos de un usuario determinado
        Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(int idUsuario);

        //Metodo para eliminar un proyecto - elimina las estaciones base y sensores asociados al proyecto
        Task<bool> EliminarProyecto(int idProyecto);
    }
}
