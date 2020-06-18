using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalWebLogin.ViewModel
{
    public class UsuarioAutenticadoViewModel : ComponentBase
    {
        protected string idUsuario { get; private set; }
        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var usuario = (await authenticationStateTask).User;

            if (!usuario.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("Identity/Account/Login");
            }
            else
            {
                if (string.IsNullOrEmpty(idUsuario))
                {
                    idUsuario = usuario.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
            }
        }
    }
}
