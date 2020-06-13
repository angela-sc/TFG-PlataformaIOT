using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Interfaces
{
    public interface IServicioSeguridad
    {
        EntidadPeticion ToEntidadPeticion(EntidadPeticionSegura peticionSegura);
        EntidadPeticionSegura ToEntidadPeticionSegura(EntidadPeticion peticion);
    }
}
