using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Frontend.Pages.Clientes
{
    [Authorize(Roles = "Admin")]
    public partial class TipoClienteCreate
    {
        private TipoClienteForm? tipoClienteForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private TipoCliente tipoCliente = new();

        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/tiposclientes", tipoCliente);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }

            Return();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.Center,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }

        private void Return()
        {
            tipoClienteForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/tiposclientes");
        }
    }
}
