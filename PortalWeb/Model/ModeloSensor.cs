using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Model
{
    public class ModeloSensor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        public string Nombre { get; set; }
        //Coordenadas del sensor - public SqlGeography Location { get; set; }
        //public double Longitud { get; set; }
        //public double Latitud { get; set; }
        //public int FK_IdEstacionBase { get; set; }
    }
}
