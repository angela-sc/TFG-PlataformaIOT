using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadSensor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        //Coordenadas del sensor - public SqlGeography Location { get; set; }
        //public double Longitud { get; set; } || public double Latitud { get; set; }  
        public string Longitud { get; set; }
        public string Latitud { get; set; }
        public int FK_IdEstacionBase { get; set; }
    }
}
