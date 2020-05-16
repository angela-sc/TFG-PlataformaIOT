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
        [Required]
        [MaxLength(15)]
        //[RegularExpression(@"^[0-9]")] //copmprobar que sea solo numerico
        [Range(-180, 180, ErrorMessage = "La longitud debe estar representada entre 0° y 180° positivos (este) o negativos (oeste).")]
        public string Longitud { get; set; }

        [Required]
        [MaxLength(15)]
        //[RegularExpression(@"^[0-9]")]
        [Range(-90, 90, ErrorMessage = "La latitud debe estar representada entre 0° y 90° positivos (norte) o negativos(sur).")]
        public double Latitud { get; set; }
        public int FK_IdEstacionBase { get; set; }
    }
}
