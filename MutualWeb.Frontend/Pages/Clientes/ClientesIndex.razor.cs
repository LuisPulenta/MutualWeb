using CurrieTechnologies.Razor.SweetAlert2;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Shared.Entities.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MutualWeb.Shared.DTOs;


namespace MutualWeb.Frontend.Pages.Clientes
{
    [Authorize(Roles = "Admin")]
    public partial class ClientesIndex
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;

        private int currentPage = 1;
        private int totalPages;
        private int totalRegisters;
        private bool IsLoading=false;
        private int selectedTipoCliente = 0;
        private int selectedSocio = 0;
        private int selectedAltaBaja = 0;

        public List<Cliente>? Clientes { get; set; }
        public List<TipoCliente>? TiposClientes { get; set; }
        private int? tipoCliente = null;
        private bool? socio = null;
        private bool? baja = null;

        //-----------------------------------------------------------------------------------------------
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
            await LoadTiposClientesAsync();
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
            //IsLoading = true;
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
            //IsLoading = false;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/clientes?page={page}&recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            if (tipoCliente !=null)
            {
                url += $"&TipoClienteFilter={tipoCliente}";
            }

            if (socio != null)
            {
                url += $"&SocioFilter={socio}";
            }

            if (baja != null)
            {
                url += $"&BajaFilter={baja}";
            }

            //IsLoading = true;
            var responseHttp = await Repository.GetAsync<List<Cliente>>(url);
            //IsLoading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Clientes = responseHttp.Response;
            return true;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/clientes/totalPages?recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            if (tipoCliente != null)
            {
                url += $"&TipoClienteFilter={tipoCliente}";
            }

            if (socio != null)
            {
                url += $"&SocioFilter={socio}";
            }

            if (baja != null)
            {
                url += $"&BajaFilter={baja}";
            }
                        
            //IsLoading = true;
            var responseHttp = await Repository.GetAsync<int>(url);
            //IsLoading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
            var a = 1;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task LoadTotalRegistersAsync()
        {
            var url = $"api/clientes/totalRegisters?recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            if (tipoCliente != null)
            {
                url += $"&TipoClienteFilter={tipoCliente}";
            }

            if (socio != null)
            {
                url += $"&SocioFilter={socio}";
            }

            if (baja != null)
            {
                url += $"&BajaFilter={baja}";
            }
                        
            //IsLoading = true;
            var responseHttp = await Repository.GetAsync<int>(url);
            //IsLoading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalRegisters = responseHttp.Response;
            var b = 2;
        }

        //-----------------------------------------------------------------------------------------------
        private async Task DeleteAsync(Cliente cliente)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Está seguro que quieres borrar al Cliente: {cliente.Titular}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "Cancelar",
            });

            var confirm = string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            var responseHTTP = await Repository.DeleteAsync($"api/clientes/{cliente.Id}");
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
                Background = "Gainsboro"
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con éxito.");
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

        //-----------------------------------------------------------------------------------
        private async Task LoadTiposClientesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<TipoCliente>>("api/tiposclientes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }

            TiposClientes = responseHttp.Response;
        }

        //---------------------------------------------------------------------------------------------------
        private async void TipoClienteChangedAsync(ChangeEventArgs e)
        {
            selectedTipoCliente = Convert.ToInt32(e.Value!);
            if (selectedTipoCliente == 0)
            { tipoCliente = null; }
            else
            {
                tipoCliente = Convert.ToInt32(selectedTipoCliente);
            }
            await LoadAsync();
        }

        //---------------------------------------------------------------------------------------------------
        private async void SocioChangedAsync(ChangeEventArgs e)
        {
            selectedSocio = Convert.ToInt32(e.Value!);
            if (selectedSocio == 0)
            { socio = null; };
            if (selectedSocio == 1)
            { socio = true; };
            if (selectedSocio == 2)
            { socio = false; };
            await LoadAsync();
        }

        //---------------------------------------------------------------------------------------------------
        private async void AltaBajaChangedAsync(ChangeEventArgs e)
        {
            selectedAltaBaja = Convert.ToInt32(e.Value!);
            if (selectedAltaBaja == 0)
            { baja = null; };
            if (selectedAltaBaja == 1)
            { baja = true; };
            if (selectedAltaBaja == 2)
            { baja = false; };
            await LoadAsync();
        }
    }
}
