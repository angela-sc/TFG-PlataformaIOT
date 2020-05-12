using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.Model
{
    public class ModeloEstacionBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        public string Nombre { get; set; }
        //public int FK_IdProyecto { get; set; }
    }
}
