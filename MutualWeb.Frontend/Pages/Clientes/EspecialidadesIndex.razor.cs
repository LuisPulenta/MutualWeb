using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities.Clientes;
using System.Net;

namespace MutualWeb.Frontend.Pages.Clientes
{
    [Authorize(Roles = "Admin")]
    public partial class EspecialidadesIndex
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

        public List<Especialidad>? Especialidades { get; set; }

        //-----------------------------------------------------------------------------------------------
        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<EspecialidadEdit>(string.Empty, new ModalParameters().Add("Id", id));
            }
            else
            {
                modalReference = Modal.Show<EspecialidadCreate>();
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
            var url = $"api/especialidades?page={page}&recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            IsLoading = true;
            var responseHttp = await Repository.GetAsync<List<Especialidad>>(url);
            IsLoading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Especialidades = responseHttp.Response;
            return true;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/especialidades/totalPages?recordsnumber={RecordsNumber}";
            
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
            var url = $"api/especialidades/totalRegisters";

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
        private async Task DeleteAsync(Especialidad especialidad)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaci�n",
                Text = $"�Est� seguro que quieres borrar la Especialidad: {especialidad.Nombre}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText= "Cancelar",
            });

            var confirm = string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            var responseHTTP = await Repository.DeleteAsync($"api/especialidades/{especialidad.Id}");
            if (responseHTTP.Error)
            {
                if (responseHTTP.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var mensajeError = await responseHTTP.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
                }
                return;
            }

            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.Center,
                ShowConfirmButton = true,
                Timer = 3000,
                Background = "Gainsboro",
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con �xito.");
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
