using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Frontend.Pages.Clientes
{
    [Authorize(Roles = "Admin")]
    public partial class ClientesCreate
    {
        private ClienteForm? clienteForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private Cliente cliente = new();

        private List<Especialidad>? especialidades;
        private List<TipoCliente>? tiposclientes;
        private bool loading;

        //-----------------------------------------------------------------------------------------------------------
        protected override async Task OnInitializedAsync()
        {
            await LoadEspecialidadesAsync();
            await LoadTiposClientesAsync();
        }

        //-----------------------------------------------------------------------------------------------------------
        private async Task LoadEspecialidadesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Especialidad>>("/api/especialidades/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            especialidades = responseHttp.Response;
        }

        //-----------------------------------------------------------------------------------------------------------
        private async Task LoadTiposClientesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<TipoCliente>>("/api/tiposclientes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            tiposclientes = responseHttp.Response;
        }

        //-----------------------------------------------------------------------------------------------------------
        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/clientes", cliente);
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
                Timer = 3000,
                Background = "Gainsboro",
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }

        //-----------------------------------------------------------------------------------------------------------
        private void Return()
        {
            clienteForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/clientes");
        }
    }
}