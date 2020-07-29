using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    //Clase resultado: datos del sensor en la vista de una estacion base
    public class EntidadSensorResultado
    {
        public string NombreSensor { get; set; } //nombre del sensor
        public int IdSensor { get; set; }

        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public float Temperatura { get; set; }
        public float Humedad { get; set; }
        public DateTime Fecha { get; set; } //fecha de la última medicion

        public int fk_idestacionbase { get; set; }
    }
}
