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

    }
}
