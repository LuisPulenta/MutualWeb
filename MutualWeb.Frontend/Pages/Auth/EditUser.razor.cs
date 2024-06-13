using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Frontend.Services;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities;

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
            var responseHttp = await Repository.PutAsync<User, TokenDTO>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            //await LoginService.LoginAsync(responseHttp.Response!.Token);
            NavigationManager.NavigateTo("/usuarios");
        }
    }
}