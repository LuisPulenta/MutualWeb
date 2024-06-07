using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Frontend.Pages.Clientes
{
    public partial class EspecialidadCreate
    {
        private EspecialidadForm? especialidadForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private Especialidad especialidad = new();

        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/especialidades", especialidad);
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
            especialidadForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/especialidades");
        }
    }
}
