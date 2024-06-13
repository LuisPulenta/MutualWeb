using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using MutualWeb.Shared.DTOs;
using System.Net;

namespace MutualWeb.Frontend.Pages.Auth
{
    [Authorize(Roles = "Admin")]
    public partial class UsuariosIndex
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        private int currentPage = 1;
        private int totalPages;

        public List<User>? Usuarios { get; set; }

        //-----------------------------------------------------------------------------------------------
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        //-----------------------------------------------------------------------------------------------
        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        //-----------------------------------------------------------------------------------------------
        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        //-----------------------------------------------------------------------------------------------
        private async Task<bool> LoadListAsync(int page)
        {
            var url = $"api/accounts/all?page={page}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<User>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Usuarios = responseHttp.Response;
            return true;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task LoadPagesAsync()
        {
            var url = "api/accounts/totalPages";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"?filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task DeleteAsync(string id)
        {
            var userResponseHppt = await Repository.GetAsync<User>("/api/accounts");
            User userLogged = userResponseHppt.Response!;

            if (userLogged.Id == id)
            {
                var messageError = "No se puede borrar a uno mismo!!!";
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }


            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "¿Realmente deseas eliminar el registro?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });

            var confirm = string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            var httpResponse = await repository.DeleteAsync($"/api/accounts/{id}");

            if (httpResponse.Error)
            {
                if (httpResponse.HttpResponseMessage.StatusCode != HttpStatusCode.NotFound)
                {
                    var messageError = await httpResponse.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                    return;
                }
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.Center,
                ShowConfirmButton = true,
                Timer = 3000,
                Background = "Gainsboro",
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro eliminado con éxito.");
            await LoadAsync();
        }
    
        //-----------------------------------------------------------------------------------------------
        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }

        //-----------------------------------------------------------------------------------------------
        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }
    }
}