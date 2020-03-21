using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadPeticion //representa la info enviada por la EB al Servidor
    {
        public string EstacionBase { get; set; }
        public string Sensor { get; set; }
        public IEnumerable<EntidadDatoBase> Datos { get; set; }
    }
}
