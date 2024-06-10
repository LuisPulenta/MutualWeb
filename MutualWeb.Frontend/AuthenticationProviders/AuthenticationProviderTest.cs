using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace MutualWeb.Frontend.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var user = new ClaimsIdentity(authenticationType: "test");
            var admin = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("FirstName", "Luis"),
                    new Claim("LastName", "Núñez"),
                    new Claim(ClaimTypes.Name, "luis@yopmail.com"),
                    new Claim(ClaimTypes.Role, "Admin")
                },
            authenticationType: "test");


            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(admin)));
        }
    }
}
