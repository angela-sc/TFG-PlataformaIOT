using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadEstacionBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int FK_IdProyecto { get; set; } 
    }
}
