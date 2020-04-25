﻿using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioUsuario
    {
        void InsertaUsuario(EntidadUsuario usuario);
        //Task <IEnumerable<EntidadProyecto>> ObtenerProyectosAsync(int idUsuario);
    }
}
