using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalWebLogin.Data;
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
        protected bool autorizado { get; private set; }

        [Parameter]
        public string idEstacionBase { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected virtual async Task OnSecureParameterSetAsync() { }

        protected sealed override async Task OnParametersSetAsync()
        {
            var usuario = (await authenticationStateTask).User;

            if (!usuario.Identity.IsAuthenticated)
            {
                autorizado = false;
                NavigationManager.NavigateTo("Identity/Account/Login");
            }
            else if (string.IsNullOrEmpty(idUsuario))
            {
                idUsuario = usuario.FindFirst(ClaimTypes.NameIdentifier).Value;
                autorizado = true;
            }

            if(!string.IsNullOrEmpty(idEstacionBase))
            {
                Int32.TryParse(idEstacionBase, out int idEb);
                var servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();
                autorizado = await servicioEstacionBase.Autorizado(idUsuario, idEb);
            }

            await OnSecureParameterSetAsync();
        }
    }
}
