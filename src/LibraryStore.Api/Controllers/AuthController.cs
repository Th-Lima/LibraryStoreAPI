using LibraryStore.Api.Dtos.AuthUserDtos;
using LibraryStore.Api.Extensions;
using LibraryStore.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LibraryStore.Api.Controllers
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(INotifier notifier, 
                              SignInManager<IdentityUser> signInManager, 
                              UserManager<IdentityUser> userManager, 
                              IOptions<AppSettings> appSettings) : base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var identityUser = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, registerUserDto.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identityUser, isPersistent: false);

                return CustomResponse(GenerateJwt());
            }

            foreach(var error in result.Errors)
            {
                NotificationErro(error.Description);
            }

            return CustomResponse(registerUserDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, true);

            if (result.Succeeded)
                return CustomResponse(GenerateJwt());

            if (result.IsLockedOut)
            {
                NotificationErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUserDto);
            }

            NotificationErro("Usuário e Senha incorretos");
            return CustomResponse(loginUserDto);
        }

        private string GenerateJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidatedOn,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
