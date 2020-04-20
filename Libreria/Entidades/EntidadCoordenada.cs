using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    //Entidad que relaciona cada sensor con sus coordenadas
    public class EntidadCoordenada
    {
        public string nombreSensor { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    }
}
