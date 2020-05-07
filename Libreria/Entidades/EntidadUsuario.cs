using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadUsuario
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public Byte[] Contrasenya { get; set; }
    }
}
