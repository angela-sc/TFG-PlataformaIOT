using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebLogin.Model
{
    public class ModeloEstacionBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        [MaxLength(10, ErrorMessage = "El nombre debe tener como máximo {0} caracteres")]
        public string Nombre { get; set; }
        public int FK_IdProyecto { get; set; }
    }
}
