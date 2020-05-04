using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioProyecto
    {
        void InsertaProyecto(EntidadProyecto proyecto);
        void EliminarProyecto();
        void EditarProyecto();

        //Metodo para obtener todos los proyectos de un usuario determinado
        Task<IEnumerable<EntidadProyecto>> ObtenerProyectos(int idUsuario);
    }
}
