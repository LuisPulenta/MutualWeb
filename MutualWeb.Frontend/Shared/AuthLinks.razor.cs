using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MutualWeb.Frontend.Pages.Auth;

namespace MutualWeb.Frontend.Shared
{
    public partial class AuthLinks
    {   
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private void ShowModal()
        {
            Modal.Show<Login>();
        }
    }
}
