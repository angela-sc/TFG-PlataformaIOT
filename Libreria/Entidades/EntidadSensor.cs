using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadSensor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Coordenadas del sensor - public SqlGeography Location { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        
        public int FK_basestationID { get; set; }
    }
}
