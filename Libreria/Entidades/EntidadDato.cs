using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadDato
    {
        public DateTime stamp { get; set; }
        public int FK_sensorID { get; set; }
        public float humity { get; set; }
        public float temperature { get; set; }
    }
}
