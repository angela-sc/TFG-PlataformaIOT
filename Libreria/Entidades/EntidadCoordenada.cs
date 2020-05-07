using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    //Entidad que relaciona cada sensor con sus coordenadas
    public class EntidadCoordenada
    {
        public string NombreSensor { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}
