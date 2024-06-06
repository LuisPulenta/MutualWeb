using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Frontend.Pages.Clientes
{
    public partial class TiposClientesIndex
    {
        private List<TipoCliente>? TiposClientes;

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var responseHttp = await repository.GetAsync<List<TipoCliente>>("api/tiposclientes");
            TiposClientes = responseHttp.Response;
        }

    }
}