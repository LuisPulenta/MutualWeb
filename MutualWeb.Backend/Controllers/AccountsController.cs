using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Helpers;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;

        public AccountsController(IUsersUnitOfWork usersUnitOfWork, IConfiguration configuration, DataContext context , IMailHelper mailHelper)
        {
            _usersUnitOfWork = usersUnitOfWork;
            _configuration = configuration;
            _context = context;
            _mailHelper = mailHelper;
        }

        //---------------------------------------------------------------------------------------------
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            user.IsActive = true;
            var result = await _usersUnitOfWork.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());
                var response = await SendConfirmationEmailAsync(user);
                if (response.WasSuccess)
                {
                    return NoContent();
                }

                return BadRequest(response.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        //---------------------------------------------------------------------------------------------
        private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user)
        {
            var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            return _mailHelper.SendMail(user.FullName, user.Email!,
                $"Mutual MyPJ HP - Confirmación de cuenta",
                $"<h1>Mutual MyPJ HP - Confirmación de cuenta</h1>" +
                $"<p>Para habilitar el usuario, por favor hacer clic en 'Confirmar Email':</p>" +
                $"<b><a href ={tokenLink}>Confirmar Email</a></b>");
        }

        //---------------------------------------------------------------------------------------------
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _usersUnitOfWork.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        //---------------------------------------------------------------------------------------------
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var result = await _usersUnitOfWork.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _usersUnitOfWork.GetUserAsync(model.Email);

                if (!user.IsActive)
                {
                    return BadRequest("Usuario no activo.");
                }
                return Ok(BuildToken(user));
            }
            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }
            return BadRequest("Email o contraseña incorrectos.");
        }

        //---------------------------------------------------------------------------------------------
        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        //--------------------------------------------------------------------------------------------
        [HttpGet("all")]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Users                
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => 
                    x.LastName.ToLower().Contains(pagination.Filter.ToLower())
                    || x.FirstName.ToLower().Contains(pagination.Filter.ToLower())
                    );
            }

            return Ok(await queryable
                .OrderBy(x => x.LastName)
                .Paginate(pagination)
                .ToListAsync());
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Users
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x =>
                    x.LastName.ToLower().Contains(pagination.Filter.ToLower())
                    || x.FirstName.ToLower().Contains(pagination.Filter.ToLower())
                    );
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalRegisters")]
        public async Task<ActionResult> GetTotalRegisters([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Users
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x =>
                    x.LastName.ToLower().Contains(pagination.Filter.ToLower())
                    || x.FirstName.ToLower().Contains(pagination.Filter.ToLower())
                    );
            }

            int count = await queryable.CountAsync();
            return Ok(count);
        }
        //--------------------------------------------------------------------------------------------
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            User user = await _usersUnitOfWork.GetUserAsync(new Guid(id));
            if (user == null)
            {
                return NotFound();
            }
            await _usersUnitOfWork.DeleteUserAsync(user);
            return NoContent();
        }

        //-------------------------------------------------------------------------------------------------
        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAsync(User user)
        {
            try
            {
                var currentUser = await _usersUnitOfWork.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.UserType = user.UserType;
                currentUser.IsActive=user.IsActive;

                var result = await _usersUnitOfWork.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //-------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!));
        }

        //-------------------------------------------------------------------------------------------------
        [HttpGet("/api/accounts/GetUserById/{Id}")]
        public async Task<User> GetUserById(string Id)
        {
            User user = await _usersUnitOfWork.GetUserAsync(new Guid(Id));
            return user!;
        }

        //-------------------------------------------------------------------------------------------------
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }

        //-------------------------------------------------------------------------------------------------
        [HttpPost("ResendToken")]
        public async Task<IActionResult> ResendTokenAsync([FromBody] EmailDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var response = await SendConfirmationEmailAsync(user);
            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        //-------------------------------------------------------------------------------------------------
        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var myToken = await _usersUnitOfWork.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Mutual MyPJ HP - Recuperación de contraseña",
                $"<h1>Mutual MyPJ HP - Recuperación de contraseña</h1>" +
                $"<p>Para recuperar su contraseña, por favor hacer clic en 'Recuperar Contraseña':</p>" +
                $"<b><a href ={tokenLink}>Recuperar Contraseña</a></b>");

            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        //-------------------------------------------------------------------------------------------------
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }
    }
}
