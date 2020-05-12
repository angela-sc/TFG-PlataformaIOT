using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.Model
{
    public class ModeloProyecto
    {
        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public int Id { get; set; }
    }
}
