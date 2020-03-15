using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Interfaces
{
    public interface IRepositorioSensor
    {
        void InsertaSensor(EntidadSensor sensor);
        void InsertaDato(EntidadDato dato);
    }
}
