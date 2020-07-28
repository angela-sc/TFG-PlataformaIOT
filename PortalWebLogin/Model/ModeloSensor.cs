using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PortalWebLogin.Model
{
    public class ModeloSensor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        [MaxLength(11, ErrorMessage ="El nombre debe tener como máximo {0} caracteres")]
        public string Nombre { get; set; }
        
        [Required]
        [RangeAttributeExtension(-180.0, 180.0, ErrorMessage = "La longitud debe estar representada entre 0° y 180° positivos (este) o negativos (oeste) y como máximo debe tener 15 dígitos..")]
        public double Longitud { get; set; }

        [Required]
        [RangeAttributeExtension(-90.0, 90.0, ErrorMessage = "La latitud debe estar representada entre 0° y 90° positivos (norte) o negativos(sur) y como máximo debe tener 15 dígitos.")]
        public double Latitud { get; set; }

        public int FK_IdEstacionBase { get; set; }
    }

    public class RangeAttributeExtension : RangeAttribute
    {
        public RangeAttributeExtension(double minimum, double maximum) : base(minimum, maximum) { }

        public override bool IsValid(object value) => ((value.ToString().StartsWith("-") && value.ToString().Length <= 17) || value.ToString().Length <= 16) ? (base.IsValid(value)) : false;
    }
}