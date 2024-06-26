using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Frontend.Services;
using MutualWeb.Shared.DTOs;

namespace MutualWeb.Frontend.Pages.Auth
{
    public partial class Login
    {
        private LoginDTO loginDTO = new();
        private bool wasClose;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        //-------------------------------------------------------------------------------------------
        private async Task CloseModalAsync()
        {
            wasClose = true;
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }

        //-------------------------------------------------------------------------------------------
        private async Task LoginAsync()
        {
            if (wasClose)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            var responseHttp = await Repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await LoginService.LoginAsync(responseHttp.Response!.Token);


            if (loginDTO.Password=="123456")
            {
                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.Center,
                    ShowConfirmButton = true,
                    Timer = 3000,
                    Background = "Gainsboro",
                });
                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambie su Contrase�a 123456 por una m�s segura.");
            }
            NavigationManager.NavigateTo("/");
        }
    }
}
