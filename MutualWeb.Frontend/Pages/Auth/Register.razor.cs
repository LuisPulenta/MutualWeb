using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MutualWeb.Frontend.Repositories;
using MutualWeb.Frontend.Services;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Enums;

namespace MutualWeb.Frontend.Pages.Auth
{
    public partial class Register
    {
        private UserDTO userDTO = new();

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        private async Task CreateUserAsync()
        {
            userDTO.UserName = userDTO.Email;
           

            if (userDTO.RolId == 1)
            {
                userDTO.UserType = UserType.Admin;
            }
            if (userDTO.RolId == 2)
            {
                userDTO.UserType = UserType.User;
            }

            var responseHttp = await Repository.PostAsync<UserDTO>("/api/accounts/CreateUser", userDTO);

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await SweetAlertService.FireAsync("Confirmación", "La cuenta ha sido creada con éxito. Se ha enviado un correo electrónico con las instrucciones para activar el usuario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/usuarios");
        }

        protected override void OnInitialized()
        {
            userDTO.Password = "123456";
        }
    }
}


