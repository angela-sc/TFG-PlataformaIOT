using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    //Clase resultado: datos del sensor en la vista de una estacion bae
    public class EntidadSensorResultado
    {
        public string NombreSensor { get; set; } //nombre del sensor
        public SqlGeography Coordenada { get; set; } //coordenadas del sensor
        public float Temperatura { get; set; }
        public float Humedad { get; set; }
        public DateTime Fecha { get; set; } //fecha de la última medicion
    }
}
