using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Pages.Clientes;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities;
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
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private int currentPage = 1;
        private int totalPages;
        private int totalRegisters;
        private bool IsLoading;

        public List<User>? Usuarios { get; set; }

        //-----------------------------------------------------------------------------------------------
        private async Task ShowModalAsync(string Id = "", bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<EditUser>(string.Empty, new ModalParameters().Add("UserId", Id));
            }
            else
            {
                modalReference = Modal.Show<Register>();
            }

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadAsync();
            }
        }

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
                await LoadTotalRegistersAsync();
            }
        }

        //-----------------------------------------------------------------------------------------------
        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/accounts/all?page={page}&recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            IsLoading = true;
            var responseHttp = await Repository.GetAsync<List<User>>(url);
            IsLoading = false;

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
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/accounts/totalPages?recordsnumber={RecordsNumber}";
            
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
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
        private async Task LoadTotalRegistersAsync()
        {
            var url = $"api/accounts/totalRegisters";

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
            totalRegisters = responseHttp.Response;
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
        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        //-----------------------------------------------------------------------------------------------
        private async Task FilterCallBack(string filter)
        {
            Filter = filter;
            await ApplyFilterAsync();
            StateHasChanged();
        }
        
        //-----------------------------------------------------------------------------------------------
        private async Task SelectedRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        //-----------------------------------------------------------------------------------------------
        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 10;
            }
        }
    }
}