using Libreria.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Interfaces
{
    public interface IRepositorioSensor
    {
        void InsertaSensor(EntidadSensor sensor);
        Task InsertaDato(EntidadDato dato);
    }
}
