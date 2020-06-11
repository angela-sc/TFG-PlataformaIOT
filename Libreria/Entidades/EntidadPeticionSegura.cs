using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Libreria.Entidades
{
    public class EntidadPeticionSegura
    {
        public string Clave { get; set; } //clave simetrica para cifrar/descifrar la peticion
        public string IV { get; set; } //vector de inicialización 
        public byte[] Peticion { get; set; } //EntidadPeticion {proyecto, estacion base, sensor y datos} en string
    }
}
