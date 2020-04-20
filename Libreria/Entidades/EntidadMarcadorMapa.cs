using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadMarcadorMapa
    {
        public string Name { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        public EntidadMarcadorMapa(string nombre, double latitud, double longitud)
        {
            this.Name = nombre;
            this.Latitud = latitud;
            this.Longitud = longitud;
        }
    }
}
