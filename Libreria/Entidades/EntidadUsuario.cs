using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    class EntidadUsuario
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public Byte[] password { get; set; }
        public int FK_projectID { get; set; }
    }
}
