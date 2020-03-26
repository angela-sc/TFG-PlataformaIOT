using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Interfaces
{
    public interface IRepositorioUsuario
    {
        void InsertaUsuario(EntidadUsuario usuario);
    }
}
