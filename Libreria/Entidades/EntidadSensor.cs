using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadSensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public SqlGeography location { get; set; }
        public int FK_basestationID { get; set; }
    }
}
