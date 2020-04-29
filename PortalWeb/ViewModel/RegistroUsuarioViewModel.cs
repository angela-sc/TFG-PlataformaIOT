using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class RegistroUsuarioViewModel : ComponentBase
    {
        [Required]
        [StringLength(10, ErrorMessage = "Nombre demasiado largo.")]
        public string name { get; set; }

        [Required]
        public string surname { get; set; }

        [Required]
        [EmailAddress] //valida que la propiedad tiene forma de correo electrónico
        public string email { get; set; }

        [Required]
        public string password { get; set; }

    }
}
