using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.Model
{
    public class Proyecto
    {
        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}
