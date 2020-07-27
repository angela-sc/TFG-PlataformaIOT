using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IServicioUsuario
    {
        Task<bool> RegistraUsuario(EntidadUsuario usuario);
    }
}
