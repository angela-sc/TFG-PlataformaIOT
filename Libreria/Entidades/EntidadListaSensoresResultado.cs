using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    //Clase resultado: datos del sensor en la vista de una estacion bae
    public class EntidadListaSensoresResultado
    {
        public string nombreSensor;
        public float ultimaTemperatura;
        public DateTime fechaTemperatura;
        public float ultimaHumedad;
        public DateTime fechaHumedad;

    }
}
