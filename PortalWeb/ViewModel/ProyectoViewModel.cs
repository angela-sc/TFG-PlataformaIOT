using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class ProyectoViewModel : ComponentBase
    {
        [ViewData]
        public string proyecto { get; } = "prueba";

        public void OnGet()
        {            
        }
    }
}
