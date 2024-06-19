using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Pages.Clientes;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Frontend.Services;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities;
using System.Net;

namespace MutualWeb.Frontend.Pages.Auth
{
    public partial class EditUser
    {
        private User? user;
        [Parameter] public string? UserId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        //--------------------------------------------------------------------------------------------------------|
        private void ShowModal()
        {
            Modal.Show<ChangePassword>();
        }

        //--------------------------------------------------------------------------------------------------------

        protected override async Task OnParametersSetAsync()
        {
            await LoadUserAsyc();
        }

        //--------------------------------------------------------------------------------------------------------
        private async Task LoadUserAsyc()
        {
            var responseHTTP = await Repository.GetAsync<User>($"/api/accounts/GetUserById/{UserId}");
            if (responseHTTP.Error)
            {
                if (responseHTTP.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/usuarios");
                    return;
                }
                var messageError = await responseHTTP.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            user = responseHTTP.Response;
        }

        //--------------------------------------------------------------------------------------------------------
        private async Task SaveUserAsync()
        {
            var userResponseHppt = await Repository.GetAsync<User>("/api/accounts");
            User userLogged = userResponseHppt.Response!;

            if (userLogged.Id == user!.Id && !user.IsActive)
            {
                var messageError = "No se puede desactivar a uno mismo!!!";
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }

            var responseHttp = await Repository.PutAsync<User, TokenDTO>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await BlazoredModal.CloseAsync(ModalResult.Ok());
            Return();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.Center,
                ShowConfirmButton = true,
                Timer = 3000,
                Background = "Gainsboro",
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con éxito.");

        }

        //--------------------------------------------------------------------------------------------------------
        private void Return()
        {
            NavigationManager.NavigateTo("usuarios");
        }
    }
}